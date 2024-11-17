public static class UserRepository
{
    public static User Get(string username, string passwordHash)
    {
        var users = new List<User>{
        new() { Id = 1, Username = "batman", PasswordHash = "batman", Role = "manager"},
        new() { Id = 2, Username = "robin", PasswordHash = "robin", Role = "employee"}
        };
        return users.FirstOrDefault(x => x.Username == username && x.PasswordHash == passwordHash);

    }
}