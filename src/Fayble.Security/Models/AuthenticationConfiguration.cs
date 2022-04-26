namespace Fayble.Security.Models
{
    public class AuthenticationConfiguration
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Key { get; set; }
    }
}
