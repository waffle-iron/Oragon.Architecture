using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Services.Models.Mixed
{
    public interface IServiceMixed
    {
        void OnWayMethod(string arg1, string arg2);

        void OnWayMethod(string arg1, string arg2, string arg3);

        string TwoWayMethod(string arg1, string arg2);

        string TwoWayMethod(string arg1, string arg2, string arg3);
    }
}
