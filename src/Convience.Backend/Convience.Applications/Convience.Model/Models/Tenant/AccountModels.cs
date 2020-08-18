namespace Convience.Model.Models.Tenant
{
    public class LoginViewModel
    {
        public string Account { get; set; }

        public string Password { get; set; }
    }

    public class LoginResultModel
    {
        public string Token { get; set; }
    }

    public class RegisterViewModel
    {
        public string Account { get; set; }

        public string Password { get; set; }
    }
}
