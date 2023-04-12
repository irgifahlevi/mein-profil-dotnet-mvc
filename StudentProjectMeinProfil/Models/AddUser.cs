using Microsoft.AspNetCore.Identity;

namespace StudentProjectMeinProfil.Models
{
    public class AddUser : IdentityUser
    {
        public string FullName { set; get; }
        public string Address { set; get; }
        public string ProfileUrl { set; get; }
        public string BrithDate { set; get; }
    }
}
