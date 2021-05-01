namespace Convience.Model.Models.SystemManage
{
    public record RoleViewModel(
        string Id = null,
        string Name = null,
        string Menus = null,
        string Remark = null);

    public record RoleResultModel(
        string Id = null,
        string Name = null,
        string Menus = null,
        string Remark = null);
}
