using System;

namespace Convience.Model.Models.SystemManage
{
    public record UserQueryModel : PageQueryModel
    {
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public string RoleId { get; set; }

        public int? Department { get; set; }

        public int? Position { get; set; }
    }

    public record UserViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public int Sex { get; set; }

        public string Avatar { get; set; }

        public string Name { get; set; }

        public string RoleIds { get; set; }

        public string PositionIds { get; set; }

        public string DepartmentId { get; set; }

        public bool IsActive { get; set; }

        public string Password { get; set; }
    }

    public record UserResultModel : UserViewModel
    {
        public DateTime CreatedTime { get; set; }

        public string RoleName { get; set; }

        public string PositionName { get; set; }

        public string DepartmentName { get; set; }
    }

    public record UserPasswordModel(int id, string Password);
}
