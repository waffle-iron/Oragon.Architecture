using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oragon.Architecture.Services;

namespace Oragon.Architecture.Tests.Services
{
    [TestClass]
    public class OragonRabbitMQTests
    {
        #region Métodos Privados de Suporte

        private Spring.Messaging.Amqp.Rabbit.Connection.CachingConnectionFactory BuildConnectionFactory()
        {
            var connectionFactory = new Spring.Messaging.Amqp.Rabbit.Connection.CachingConnectionFactory()
            {
                Host = "oragonserver02",
                Port = 5672,
                UserName = "wcfUser",
                Password = "wcfPassword",
                VirtualHost = "WCF"
            };
            return connectionFactory;
        }

        private RabbitMQServiceHost BuildHost(int concurrentConsumers, object serviceInstance, Type serviceInterface)
        {
            RabbitMQServiceHost host = new RabbitMQServiceHost()
            {
                ConnectionFactory = this.BuildConnectionFactory(),
                AutoStart = false,
                ConcurrentConsumers = concurrentConsumers,
                Service = serviceInstance,
                ServiceInterface = serviceInterface,
                DropQueueAfterLastListenerStop = false
            };
            return host;
        }

        private RabbitMQServiceClientFactory BuildClient(Type serviceInterface)
        {
            RabbitMQServiceClientFactory clientFactory = new RabbitMQServiceClientFactory()
            {
                Timeout = 500000,
                ConnectionFactory = this.BuildConnectionFactory(),
                ServiceInterface = serviceInterface,
            };
            return clientFactory;
        }

        #endregion

        [TestMethod]
        public void OneWayTest()
        {
            RabbitMQServiceHost host = this.BuildHost(1, new Models.OneWay.ServiceOneWay(), typeof(Models.OneWay.IServiceOneWay));
            host.AfterPropertiesSet();
            RabbitMQServiceClientFactory client = this.BuildClient(typeof(Models.OneWay.IServiceOneWay));
            client.AfterPropertiesSet();
            host.Start();
            Models.OneWay.IServiceOneWay clientSvc = (Models.OneWay.IServiceOneWay)client.GetObject();
            clientSvc.OnWayMethod("a1", "a2");
            clientSvc.OnWayMethod("b1", "b2", "b3");
            host.Stop();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "RaiseException")]
        public void OneWayTestException()
        {
            RabbitMQServiceHost host = this.BuildHost(1, new Models.OneWay.ServiceOneWay(), typeof(Models.OneWay.IServiceOneWay));
            host.AfterPropertiesSet();
            RabbitMQServiceClientFactory client = this.BuildClient(typeof(Models.OneWay.IServiceOneWay));
            client.AfterPropertiesSet();
            host.Start();
            Models.OneWay.IServiceOneWay clientSvc = (Models.OneWay.IServiceOneWay)client.GetObject();
            clientSvc.RaiseException();
            host.Stop();
        }

        [TestMethod]
        public void TwoWayTest()
        {
            RabbitMQServiceHost host = this.BuildHost(1, new Models.TwoWay.ServiceTwoWay(), typeof(Models.TwoWay.IServiceTwoWay));
            host.AfterPropertiesSet();
            RabbitMQServiceClientFactory client = this.BuildClient(typeof(Models.TwoWay.IServiceTwoWay));
            client.AfterPropertiesSet();
            host.Start();
            Models.TwoWay.IServiceTwoWay clientSvc = (Models.TwoWay.IServiceTwoWay)client.GetObject();
            string return1 = clientSvc.TwoWayMethod("a1", "a2");
            Assert.AreEqual(return1, "arg1:'a1' | arg2:'a2'");
            string return2 = clientSvc.TwoWayMethod("b1", "b2", "b3");
            Assert.AreEqual(return2, "arg1:'b1' | arg2:'b2' | arg3:'b3'");
            host.Stop();
        }

        [TestMethod]
        public void MixedTest()
        {
            RabbitMQServiceHost host = this.BuildHost(1, new Models.Mixed.ServiceMixed(), typeof(Models.Mixed.IServiceMixed));
            host.AfterPropertiesSet();
            RabbitMQServiceClientFactory client = this.BuildClient(typeof(Models.Mixed.IServiceMixed));
            client.AfterPropertiesSet();
            host.Start();
            Models.Mixed.IServiceMixed clientSvc = (Models.Mixed.IServiceMixed)client.GetObject();
            clientSvc.OnWayMethod("a1", "a2");
            clientSvc.OnWayMethod("b1", "b2", "b3");
            string return1 = clientSvc.TwoWayMethod("c1", "c2");
            Assert.AreEqual(return1, "arg1:'c1' | arg2:'c2'");
            string return2 = clientSvc.TwoWayMethod("d1", "d2", "d3");
            Assert.AreEqual(return2, "arg1:'d1' | arg2:'d2' | arg3:'d3'");
            host.Stop();
        }

        [TestMethod]
        public void MultithreadingTest()
        {
            RabbitMQServiceHost host = this.BuildHost(10, new Models.Multithreading.MultithreadingService(), typeof(Models.Multithreading.IMultithreadingService));
            host.AfterPropertiesSet();
            RabbitMQServiceClientFactory client = this.BuildClient(typeof(Models.Multithreading.IMultithreadingService));
            client.AfterPropertiesSet();
            host.Start();
            Models.Multithreading.IMultithreadingService clientSvc = (Models.Multithreading.IMultithreadingService)client.GetObject();

            List<System.Threading.Thread> threads = new List<System.Threading.Thread>();

            for (int i = 1; i <= 10; i++)
            {
                System.Threading.Thread t1 = new System.Threading.Thread(() => { System.Diagnostics.Debug.WriteLine(clientSvc.MultithreadingTest()); });
                t1.Start();
                threads.Add(t1);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            host.Stop();
        }
    }
}