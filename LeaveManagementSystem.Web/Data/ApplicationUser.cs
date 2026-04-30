namespace LeaveManagementSystem.Web.Data;

//inherit other Properties from IdentityUser Class
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }

}
