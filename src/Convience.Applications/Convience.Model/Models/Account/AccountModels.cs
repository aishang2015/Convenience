namespace Convience.Model.Models.Account
{
    public class CaptchaResultModel
    {
        public string CaptchaKey { get; set; }

        public string CaptchaData { get; set; }
    }

    public class ChangePwdViewModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }

    public class LoginResultModel
    {
        public string Name { get; set; }

        public string Avatar { get; set; }

        public string Token { get; set; }

        public string Identification { get; set; }

        public string Routes { get; set; }
    }

    public class LoginViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string CaptchaKey { get; set; }

        public string CaptchaValue { get; set; }
    }
}
