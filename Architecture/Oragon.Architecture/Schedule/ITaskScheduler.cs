using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Schedule
{
    public interface ITaskScheduler
    {
        void ScheduleTask(Task taskToSchedule);
    }
}
