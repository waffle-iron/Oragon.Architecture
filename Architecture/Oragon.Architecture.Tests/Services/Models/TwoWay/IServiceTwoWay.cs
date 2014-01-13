using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Services.Models.TwoWay
{
    public interface IServiceTwoWay
    {
        string TwoWayMethod(string arg1, string arg2);

        string TwoWayMethod(string arg1, string arg2, string arg3);
    }
}
