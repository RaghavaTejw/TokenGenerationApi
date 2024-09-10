using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokenGenerationApi.Models
{
    public class LoginModel
    {
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public string? Permission {  get; set; }

        //public ICollection<Roles>? Roles { get; set; }
       
    }
}
