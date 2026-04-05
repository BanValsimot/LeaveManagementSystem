using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data
{
    public class LeaveType
    {
        //primary key
        public int Id { get; set; }
        [Column(TypeName ="nvarchar(150)")]
        public string Name { get; set; }
        //float - half a day off
        public float NumberOfDays { get; set; }
    }
}

