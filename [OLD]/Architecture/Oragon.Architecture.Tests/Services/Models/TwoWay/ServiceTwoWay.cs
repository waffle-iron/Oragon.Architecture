using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Services.Models.TwoWay
{
    public class ServiceTwoWay : IServiceTwoWay
    {
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
