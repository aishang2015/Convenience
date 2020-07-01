namespace Convience.Model.Models.GroupManage
{
    public class EmployeeQueryModel
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }
    }

    public class EmployeeViewModel
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; }

        public string Avatar { get; set; }

        public string Name { get; set; }

        public int Sex { get; set; }

        public string PositionIds { get; set; }

        public string DepartmentId { get; set; }
    }

    public class EmployeeResultModel : EmployeeViewModel
    {
    }
}
