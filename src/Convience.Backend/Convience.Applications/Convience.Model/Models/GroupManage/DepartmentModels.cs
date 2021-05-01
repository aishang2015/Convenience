namespace Convience.Model.Models.GroupManage
{
    public record DepartmentResultModel : DepartmentViewModel
    {
        public string LeaderName { get; set; }
    }

    public record DepartmentViewModel
    {
        public int? Id { get; set; }

        public string UpId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public int? LeaderId { get; set; }

        public int Sort { get; set; }
    }


}
