using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Services.Models.Mixed
{
    public class ServiceMixed : IServiceMixed
    {
        public void OnWayMethod(string arg1, string arg2)
        {
            string text = string.Format("arg1:'{0}' | arg2:'{1}'", arg1, arg2);
            Console.WriteLine(text);
        }

        public void OnWayMethod(string arg1, string arg2, string arg3)
        {
            string text = string.Format("arg1:'{0}' | arg2:'{1}' | arg3:'{2}'", arg1, arg2, arg3);
            Console.WriteLine(text);
        }

        public string TwoWayMethod(string arg1, string arg2)
        {
            return string.Format("arg1:'{0}' | arg2:'{1}'", arg1, arg2);
        }

        public string TwoWayMethod(string arg1, string arg2, string arg3)
        {
            return string.Format("arg1:'{0}' | arg2:'{1}' | arg3:'{2}'", arg1, arg2, arg3);
        }
    }
}
