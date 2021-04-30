namespace Convience.Model.Models.Account
{
    public record CaptchaResultModel(string CaptchaKey, string CaptchaData);

    public record ChangePwdViewModel(string OldPassword, string NewPassword);

    public record LoginResultModel(
        string Name,
        string Avatar,
        string Token,
        string Identification,
        string Routes);

    public record LoginViewModel(
        string UserName,
        string Password,
        string CaptchaKey,
        string CaptchaValue);

    public record ValidateCredentialsResultModel(
        string Token,
        string Name,
        string Avatar,
        string RoleIds);
}
