namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    //inherits ID from the Base Class
    public class LeaveTypeEditVM : BaseLeaveTypeVM
    {
        [Required]
        [Length(4, 150, 
            ErrorMessage = "You have vioalated the length requirements.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.5, 90)]
        //displayed name in the Browser for this Property
        [Display(Name = "Maximum Allocation of Days")]
        public float Days { get; set; }
    }
}


