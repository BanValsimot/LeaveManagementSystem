namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    //inherits ID from the Base Class
    public class LeaveTypeReadOnlyVM : BaseLeaveTypeVM
    {
        //avoid null-warning
        public string Name { get; set; } = string.Empty;
        //float defaults to zero when no value is assigned
        //displayed name in the Browser for this Property
        [Display(Name = "Maximum Allocation of Days")]
        public float Days { get; set; }
    }
}

