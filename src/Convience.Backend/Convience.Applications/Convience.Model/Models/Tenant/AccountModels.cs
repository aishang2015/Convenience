namespace Convience.Model.Models.Tenant
{
    public class TenantLoginViewModel
    {
        public string Account { get; set; }

        public string Password { get; set; }
    }

    public class TenantLoginResultModel
    {
        public string Token { get; set; }
    }

    public class RegisterViewModel
    {
        public string Account { get; set; }

        public string Password { get; set; }
    }
}
