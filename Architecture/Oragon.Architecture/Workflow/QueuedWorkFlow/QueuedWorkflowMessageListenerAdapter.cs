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

namespace Oragon.Architecture.Workflow.QueuedWorkFlow
{
	public class QueuedWorkflowMessageListenerAdapter : IMessageListener, IChannelAwareMessageListener
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
		private volatile bool mandatoryPublish;
		private volatile bool immediatePublish;
		private volatile IMessagePropertiesConverter messagePropertiesConverter = new DefaultMessagePropertiesConverter();

		public object HandlerObject { get; set; }

		public string Encoding { get; set; }

		public string DefaultListenerMethod { get; set; }

		public string ResponseRoutingKey { get; set; }

		public string ResponseExchange { get; set; }

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
				this.HandleListenerException(ex);
			}
		}

		public void OnMessage(Message message, IModel channel)
		{
			if (this.HandlerObject != this)
			{
				if (this.HandlerObject is IChannelAwareMessageListener)
				{
					if (channel != null)
					{
						((IChannelAwareMessageListener)this.HandlerObject).OnMessage(message, channel);
						return;
					}
					if (!(this.HandlerObject is IMessageListener))
					{
						throw new AmqpIllegalStateException("MessageListenerAdapter cannot handle a IChannelAwareMessageListener delegate if it hasn't been invoked with a Channel itself");
					}
				}
				if (this.HandlerObject is IMessageListener)
				{
					((IMessageListener)this.HandlerObject).OnMessage(message);
					return;
				}
			}
			object messageObject = this.ExtractMessage(message);
			if (this.HandlerObject is Func<string, string> && messageObject is string)
			{
				string text = ((Func<string, string>)this.HandlerObject)((string)messageObject);
				if (text != null)
				{
					this.HandleResult(text, message, channel);
					return;
				}
			}
			string listenerMethodName = this.GetListenerMethodName(message, messageObject);
			if (listenerMethodName == null)
			{
				throw new AmqpIllegalStateException("No default listener method specified: Either specify a non-null value for the 'DefaultListenerMethod' property or override the 'GetListenerMethodName' method.");
			}
			object[] arguments = this.BuildListenerArguments(messageObject);
			object listenerMethodResult = this.InvokeListenerMethod(listenerMethodName, arguments);
			if (listenerMethodResult != null)
			{
				this.HandleResult(listenerMethodResult, message, channel);
			}
			else
			{
				this.HandleResult(messageObject, message, channel);
			}
		}

		protected virtual void InitDefaultStrategies()
		{
			this.DefaultListenerMethod = "HandleMessage";
			this.ResponseRoutingKey = string.Empty;
			this.ResponseExchange = string.Empty;
			this.Encoding = "UTF-8";
			this.MessageConverter = new SimpleMessageConverter();
		}
		protected virtual void HandleListenerException(System.Exception ex)
		{
			QueuedWorkflowMessageListenerAdapter.Logger.Error(delegate(FormatMessageHandler m)
			{
				m.Invoke("Listener execution failed", new object[0]);
			}, ex);
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
		protected virtual string GetListenerMethodName(Message originalMessage, object extractedMessage)
		{
			return this.DefaultListenerMethod;
		}
		protected object[] BuildListenerArguments(object extractedMessage)
		{
			return new object[]
			{
				extractedMessage
			};
		}
		protected object InvokeListenerMethod(string methodName, object[] arguments)
		{
			object result;
			try
			{
				MethodInvoker methodInvoker = new MethodInvoker();
				methodInvoker.TargetObject = this.HandlerObject;
				methodInvoker.TargetMethod = methodName;
				methodInvoker.Arguments = arguments;
				methodInvoker.Prepare();
				object obj = methodInvoker.Invoke();
				if (obj == MethodInvoker.Void)
				{
					result = null;
				}
				else
				{
					result = obj;
				}
			}
			catch (System.Reflection.TargetInvocationException ex)
			{
				System.Exception innerException = ex.InnerException;
				if (innerException is System.IO.IOException)
				{
					throw new AmqpIOException(innerException);
				}
				throw new ListenerExecutionFailedException("Listener method '" + methodName + "' threw exception", innerException);
			}
			catch (System.Exception innerException2)
			{
				throw new ListenerExecutionFailedException(this.BuildInvocationFailureMessage(methodName, arguments), innerException2);
			}
			return result;
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
		protected virtual void HandleResult(object result, Message request, IModel channel)
		{
			if (channel != null)
			{
				QueuedWorkflowMessageListenerAdapter.Logger.Debug(delegate(FormatMessageHandler m)
				{
					m.Invoke("Listener method returned result [{0}] - generating response message for it", new object[]
					{
						result
					});
				});
				Message message = this.BuildMessage(channel, result);
				//this.PostProcessResponse(request, message);
				Address replyToAddress = this.GetReplyToAddress(request);
				this.SendResponse(channel, replyToAddress, message);
				return;
			}
			QueuedWorkflowMessageListenerAdapter.Logger.Warn(delegate(FormatMessageHandler m)
			{
				m.Invoke("Listener method returned result [{0}]: not generating response message for it because of no Rabbit Channel given", new object[]
				{
					result
				});
			});
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
		protected virtual Address GetReplyToAddress(Message request)
		{
			Address address = new Address(null, this.ResponseExchange, this.ResponseRoutingKey);
			return address;
		}
		protected virtual void SendResponse(IModel channel, Address replyTo, Message message)
		{
			this.PostProcessChannel(channel, message);
			try
			{
				QueuedWorkflowMessageListenerAdapter.Logger.Debug(delegate(FormatMessageHandler m)
				{
					m.Invoke("Publishing response to exchanage = [{0}], routingKey = [{1}]", new object[]
					{
						replyTo.ExchangeName,
						replyTo.RoutingKey
					});
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
