namespace Package.Shared.ValueObjects;

public class UserClaimsValue
{
    public Guid Id { get; set; }
    
    public int SubId { get; set; }
    
    public string UserName { get; set; } = null!;
    
    public string FullName { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
}