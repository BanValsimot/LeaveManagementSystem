using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    public class LeaveTypeReadOnlyVM : BaseLeaveTypeVM
    {
        //avoid null-warning
        public string Name { get; set; } = string.Empty;
        //float defaults to zero when no value is assigned
        [Display(Name = "Maximum Allocation of Days")]
        public float Days { get; set; }
    }
}

