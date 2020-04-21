namespace Convience.Model.Models.GroupManage
{
    public class EmployeeQuery
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }
    }
}
