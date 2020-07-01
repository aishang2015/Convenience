using Convience.Entity.Data;
using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity.WorkFlows
{
    [Entity(DbContextType = typeof(SystemIdentityDbContext))]
    public class WorkFlowCondition
    {
        public int Id { get; set; }

        public string SourceId { get; set; }

        public string TargetId { get; set; }

        public int FormControlId { get; set; }

        public CompareModeEnum CompareMode { get; set; }

        public string CompareValue { get; set; }

        #region

        public int WorkFlowId { get; set; }

        public WorkFlow WorkFlow { get; set; }

        #endregion
    }

    public enum CompareModeEnum
    {
        Equal = 1,
        Greater = 2,
        Smaller = 3,
        EqualOrGreater = 4,
        EqualOrSmaller = 5,
    }
}
