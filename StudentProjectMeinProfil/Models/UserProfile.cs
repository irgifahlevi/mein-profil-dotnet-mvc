namespace StudentProjectMeinProfil.Models
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { set; get; }
        public string ProfileUrl { set; get; }
        public string BrithDate { set; get; }
        public string PhoneNumber { set; get; }
    }
}
