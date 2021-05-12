namespace Convience.Model.Models.SystemManage
{
    public record MenuViewModel
    (
        int? Id = null,
        string UpId = null,
        string Name = null,
        string Identification = null,
        string Permission = null,
        int? Type = null,
        string Route = null,
        int? Sort = null
   );

    public record MenuResultModel : MenuViewModel
    {
    }
}
