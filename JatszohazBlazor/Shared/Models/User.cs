namespace JatszohazBlazor.Shared.Models
{
    public class User
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public UserPermissions Permission { get; set; }

        public enum UserPermissions
        {
            None,
            Admin
        }
    }
}
