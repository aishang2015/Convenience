using System.ComponentModel.DataAnnotations;

namespace backend.model.backend.api.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
