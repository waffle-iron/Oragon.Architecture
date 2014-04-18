using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Services.Models.OneWay
{
    public class ServiceOneWay : IServiceOneWay
    {
        public void OneWayMethod(string arg1, string arg2)
        {
            string text = string.Format("arg1:'{0}' | arg2:'{1}'", arg1, arg2);
            Console.WriteLine(text);
        }

        public void OneWayMethod(string arg1, string arg2, string arg3)
        {
            string text = string.Format("arg1:'{0}' | arg2:'{1}' | arg3:'{2}'", arg1, arg2, arg3);
            Console.WriteLine(text);
        }

        public void RaiseException()
        {
            throw new InvalidOperationException("RaiseException");
        }
    }
}
