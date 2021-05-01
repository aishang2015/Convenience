namespace Convience.Model.Models.GroupManage
{
    public record PositionQueryModel
    {
        public int Page { get; init; }

        public int Size { get; init; }
    }

    public record PositionViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int Sort { get; init; }
    }

    public record PositionResultModel : PositionViewModel
    {
    }
}
