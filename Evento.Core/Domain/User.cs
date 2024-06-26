namespace Evento.Core.Domain;

public class User : Entity
{
    private static List<string> _roles = new List<string>
    {
        "user", "admin"
    };
    public string Role { get; protected set; }
    public string Name { get; protected set; }
    public string Email { get; protected set; }
    public string Password { get; protected set; }
    public DateTime CreatedAt { get; protected set; }

    protected User() {}

    public User(Guid id, string role, string name,
        string email, string password) {

        Id = id;
        SetRole(role);
        SetName(name);
        SetEmail(email);
        SetPassword(password);
        CreatedAt = DateTime.UtcNow;
    }

    public void SetName(string name) {

        if(string.IsNullOrWhiteSpace(name))
        {
            throw new Exception($"User can not have an empty name.");
        }
        Name = name;
    }

    public void SetEmail(string email) {

        // todo: email validation - regular expression

        if(string.IsNullOrWhiteSpace(email))
        {
            throw new Exception($"User can not have an empty email.");
        }
        Email = email;
    }

    public void SetRole(string role) {

        if(string.IsNullOrWhiteSpace(role))
        {
            throw new Exception($"User can not have an empty role.");
        }
        role = role.ToLowerInvariant();
        if(!_roles.Contains(role))
        {
            throw new Exception($"User can not have a role: '{role}'.");
        }
        Role = role;
    }

    public void SetPassword(string password) {

        // todo: checking password strength, length, etc

        if(string.IsNullOrWhiteSpace(password))
        {
            throw new Exception($"User can not have an empty password.");
        }
        Password = password;
    }
}
