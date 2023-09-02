using System.ComponentModel.DataAnnotations;

namespace SibSalamat.Views.Account
{
    public class SignUpViewModel
    {
        [Required] public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required] public string Email { get; set; }
    }
}