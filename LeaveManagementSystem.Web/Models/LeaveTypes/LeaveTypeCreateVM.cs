using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    //no ID provided
    public class LeaveTypeCreateVM
    {
        [Required]
        [Length(4, 15, ErrorMessage = "You have vioalated the length requirements.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.5, 90)]
        [Display(Name="Maximum Allocation of Days")]
        public float Days { get; set; }
    }
}


