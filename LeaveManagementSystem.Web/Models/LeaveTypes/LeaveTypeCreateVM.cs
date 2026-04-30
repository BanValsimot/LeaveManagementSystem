namespace LeaveManagementSystem.Web.Models.LeaveTypes;

//takes 2 values from the Form -> Class specific for the Create View
public class LeaveTypeCreateVM
{
    //Database model accepts max. of 150 charachters
    //Optional parameters -> name of the parameter should be explicitly given
    [Required]
    [Length(4, 150, 
        ErrorMessage = "You have vioalated the length requirements.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.5, 90)]
    //displayed name in the Browser for this Property
    [Display(Name="Maximum Allocation of Days")]
    public float Days { get; set; }
}


