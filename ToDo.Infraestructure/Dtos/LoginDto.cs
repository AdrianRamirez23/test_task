


using System.ComponentModel.DataAnnotations;

namespace ToDo.Infraestructure.Dtos
{
    public class LoginDto
    {

        [Required(ErrorMessage = "The username is required.")]
        [StringLength(50, ErrorMessage = "The username cannot exceed 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        [StringLength(100, ErrorMessage = "The password cannot exceed 100 characters.")]
        [DataType(DataType.Password, ErrorMessage = "The password must be a valid format.")]
        public string Password { get; set; }
    }
}
