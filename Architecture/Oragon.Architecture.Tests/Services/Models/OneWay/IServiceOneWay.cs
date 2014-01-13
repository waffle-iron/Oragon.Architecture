using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Services.Models.OneWay
{
    public interface IServiceOneWay
    {
        void OneWayMethod(string arg1, string arg2);

        void OneWayMethod(string arg1, string arg2, string arg3);

        void RaiseException();
    }
}
