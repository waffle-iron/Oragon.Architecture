using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Services.Models.Multithreading
{
    public class MultithreadingService : IMultithreadingService
    {
        public string MultithreadingTest()
        {
            string messageToReturn = string.Format("Start Time = {0}", DateTime.Now.ToShortDateString());
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
            messageToReturn += string.Format(" | EndTime Time = {0}", DateTime.Now.ToShortDateString());
            return messageToReturn;
        }
    }
}
