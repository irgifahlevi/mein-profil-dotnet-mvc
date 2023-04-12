using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectMeinProfil.Models
{
    public class RegisterView
    {
        public string FullName { set; get; }
        public string UserName { set; get; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { set; get; }
        public string ProfileUrl { set; get; }
        public string BrithDate { set; get; }

    }
}
