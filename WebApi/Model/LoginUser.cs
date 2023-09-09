using System.ComponentModel.DataAnnotations;

namespace WebApi.Model
{
    public class LoginUser
    {
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
