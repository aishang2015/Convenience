namespace Convience.Model.Models.ContentManage
{
    public class DicDataResultModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }
    }

    public class DicDataViewModel
    {
        public int Id { get; set; }

        public int DicTypeId { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }
    }

    public class DicTypeViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }
    }


    public class DicTypeResultModel : DicTypeViewModel
    {
    }
}
