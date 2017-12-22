using System.Collections.Generic;

namespace Scheduler.ScheduleStealer.UserSelections
{
    public interface IStudentSelectionUpdater
    {
        IReadOnlyList<StudentSelection> Update();
    }
}