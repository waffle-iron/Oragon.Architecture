using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Schedule
{
    public interface ITaskHandler
    {
        bool HandleRequired { get; }
        
        string Name { get; }

        bool CanHandle(Task taskToAnalyse);

        void Handle(Task taskToHandle);
    }
}
