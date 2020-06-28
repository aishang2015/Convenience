namespace Convience.Model.Models.GroupManage
{
    public class PositionQueryModel
    {
        public int Page { get; set; }

        public int Size { get; set; }
    }

    public class PositionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }
    }

    public class PositionResultModel : PositionViewModel
    {
    }
}
