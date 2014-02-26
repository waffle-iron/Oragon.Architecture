using Common.Logging;
using RabbitMQ.Client;
using Spring.Messaging.Amqp.Core;
using Spring.Messaging.Amqp.Rabbit.Connection;
using Spring.Messaging.Amqp.Rabbit.Core;
using Spring.Messaging.Amqp.Rabbit.Support;
using Spring.Messaging.Amqp.Support.Converter;
using Spring.Objects.Support;
using Spring.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Spring.Messaging.Amqp;
using Spring.Messaging.Amqp.Rabbit.Listener;
using NLog;

namespace Oragon.Architecture.Workflow.QueuedWorkFlow
{
	public class QueuedWorkflowMessageListenerAdapter : IMessageListener, IChannelAwareMessageListener
	{
		private static readonly Logger Logger = NLog.LogManager.GetLogger("QueuedWorkflowMessageListenerAdapter");
		private volatile bool mandatoryPublish;
		private volatile bool immediatePublish;
		private volatile IMessagePropertiesConverter messagePropertiesConverter = new DefaultMessagePropertiesConverter();

		public object HandlerObject { get; set; }

		public string Encoding { get; set; }

		public string DefaultListenerMethod { get; set; }

		public string ResponseRoutingKey { get; set; }
		public string ResponseExchange { get; set; }

		public string ResponseFailureRoutingKey { get; set; }
		public string ResponseFailureExchange { get; set; }

		public IMessageConverter MessageConverter { get; set; }

		public bool MandatoryPublish
		{
			set
			{
				this.mandatoryPublish = value;
			}
		}
		public bool ImmediatePublish
		{
			set
			{
				this.immediatePublish = value;
			}
		}


		public QueuedWorkflowMessageListenerAdapter()
		{
			this.InitDefaultStrategies();
			this.HandlerObject = this;
		}
		public QueuedWorkflowMessageListenerAdapter(object handlerObject)
		{
			this.InitDefaultStrategies();
			this.HandlerObject = handlerObject;
		}
		public QueuedWorkflowMessageListenerAdapter(object handlerObject, IMessageConverter messageConverter)
		{
			this.InitDefaultStrategies();
			this.HandlerObject = handlerObject;
			this.MessageConverter = messageConverter;
		}

		public void OnMessage(Message message)
		{
			try
			{
				this.OnMessage(message, null);
			}
			catch (System.Exception ex)
			{
				this.HandleListenerException(ex, message);
			}
		}

		public void OnMessage(Message message, IModel channel)
		{
			object messageObject = this.ExtractMessage(message);
			if (this.DefaultListenerMethod == null)
				throw new AmqpIllegalStateException("No default listener method specified: Either specify a non-null value for the 'DefaultListenerMethod' property or override the 'GetListenerMethodName' method.");

			object[] arguments = this.BuildListenerArguments(messageObject);
			InvokeResult invokeResult = this.InvokeListenerMethod(this.DefaultListenerMethod, arguments);
			if (invokeResult.Success)
			{
				object messageToSend = invokeResult.HasValue ? invokeResult.ReturnedValue : messageObject;
				this.HandleSuccessResult(messageToSend, message, channel);
			}
			else if (invokeResult.Exception is FlowRejectAndRequeueException)
			{
				this.HandleRequeueResult(messageObject, message, channel);
			}
			else
			{
				this.HandleFailureResult(messageObject, message, channel);
			}
		}

		protected virtual void InitDefaultStrategies()
		{
			this.DefaultListenerMethod = "HandleMessage";
			this.ResponseRoutingKey = string.Empty;
			this.ResponseExchange = string.Empty;
			this.ResponseFailureRoutingKey = string.Empty;
			this.ResponseFailureExchange = string.Empty;

			this.Encoding = "UTF-8";
			this.MessageConverter = new SimpleMessageConverter();
		}
		protected virtual void HandleListenerException(System.Exception ex, Message message)
		{
			Logger.Error("Listener execution failed", ex);
		}
		private object ExtractMessage(Message message)
		{
			IMessageConverter messageConverter = this.MessageConverter;
			if (messageConverter != null)
			{
				return messageConverter.FromMessage(message);
			}
			return message;
		}



		protected object[] BuildListenerArguments(object extractedMessage)
		{
			return new object[]
			{
				extractedMessage
			};
		}

		protected InvokeResult InvokeListenerMethod(string methodName, object[] arguments)
		{
			InvokeResult returnValue = new InvokeResult();
			try
			{
				MethodInvoker methodInvoker = new MethodInvoker();
				methodInvoker.TargetObject = this.HandlerObject;
				methodInvoker.TargetMethod = methodName;
				methodInvoker.Arguments = arguments;
				methodInvoker.Prepare();
				returnValue.ReturnedValue = methodInvoker.Invoke();
			}
			catch (System.Exception ex)
			{
				returnValue.Exception = ex;
			}
			return returnValue;
		}
		private string BuildInvocationFailureMessage(string methodName, object[] arguments)
		{
			return string.Concat(new string[]
			{
				"Failed to invoke target method '",
				methodName,
				"' with argument types = [",
				StringUtils.CollectionToCommaDelimitedString<string>(this.GetArgumentTypes(arguments)),
				"], values = [",
				StringUtils.CollectionToCommaDelimitedString<object>(arguments),
				"]"
			});
		}
		private System.Collections.Generic.List<string> GetArgumentTypes(object[] arguments)
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			if (arguments != null)
			{
				for (int i = 0; i < arguments.Length; i++)
				{
					list.Add(arguments[i].GetType().ToString());
				}
			}
			return list;
		}
		protected virtual void HandleResult(object messageToSend, Message request, IModel channel, Address replyToAddress)
		{
			if (channel != null)
			{
				if (messageToSend is System.Collections.IList)
				{
					System.Collections.IList listOfMessages = (System.Collections.IList)messageToSend;
					foreach (object singleMessage in listOfMessages)
					{
						Message message = this.BuildMessage(channel, singleMessage);
						this.SendResponse(channel, replyToAddress, message);
					}
				}
				else
				{
					Message message = this.BuildMessage(channel, messageToSend);
					this.SendResponse(channel, replyToAddress, message);
				}
				return;
			}
		}

		protected virtual void HandleSuccessResult(object messageToSend, Message request, IModel channel)
		{
			Address replyToAddress = new Address(null, this.ResponseExchange, this.ResponseRoutingKey);
			this.HandleResult(messageToSend, request, channel, replyToAddress);
		}

		protected virtual void HandleRequeueResult(object messageToSend, Message request, IModel channel)
		{
			Address replyToAddress = new Address(null, this.ResponseExchange, this.ResponseRoutingKey);
			this.HandleResult(messageToSend, request, channel, replyToAddress);
		}

		protected virtual void HandleFailureResult(object messageToSend, Message request, IModel channel)
		{
			if (string.IsNullOrWhiteSpace(this.ResponseFailureExchange) == false && string.IsNullOrWhiteSpace(this.ResponseFailureRoutingKey) == false)
			{
				Address replyToAddress = new Address(null, this.ResponseFailureExchange, this.ResponseFailureRoutingKey);
				this.HandleResult(messageToSend, request, channel, replyToAddress);
			}
		}

		protected string GetReceivedExchange(Message request)
		{
			return request.MessageProperties.ReceivedExchange;
		}
		protected Message BuildMessage(IModel channel, object result)
		{
			IMessageConverter messageConverter = this.MessageConverter;
			if (messageConverter != null)
			{
				return messageConverter.ToMessage(result, new MessageProperties());
			}
			if (!(result is Message))
			{
				throw new MessageConversionException("No IMessageConverter specified - cannot handle message [" + result + "]");
			}
			return (Message)result;
		}
		protected virtual void PostProcessResponse(Message request, Message response)
		{
			string text = request.MessageProperties.CorrelationId.ToStringWithEncoding("UTF-8");
			if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(request.MessageProperties.MessageId))
			{
				text = request.MessageProperties.MessageId;
			}
			response.MessageProperties.CorrelationId = text.ToByteArrayWithEncoding("UTF-8");
		}


		protected virtual void SendResponse(IModel channel, Address replyTo, Message message)
		{
			this.PostProcessChannel(channel, message);
			try
			{
				QueuedWorkflowMessageListenerAdapter.Logger.Debug("Publishing response to exchanage = [{0}], routingKey = [{1}]", new object[]
					{
						replyTo.ExchangeName,
						replyTo.RoutingKey
					});
				channel.BasicPublish(replyTo.ExchangeName, replyTo.RoutingKey, this.mandatoryPublish, this.immediatePublish, this.messagePropertiesConverter.FromMessageProperties(channel, message.MessageProperties, this.Encoding), message.Body);
			}
			catch (System.Exception ex)
			{
				throw RabbitUtils.ConvertRabbitAccessException(ex);
			}
		}
		protected virtual void PostProcessChannel(IModel channel, Message response)
		{
		}
	}

}
