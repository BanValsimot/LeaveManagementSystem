using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data;

//Name of the Entity / Data Model
public class LeaveType
{
    //primary key
    //combo of int + ID -> automatically recognied as PK
    //[key] -> when not following convention
    public int Id { get; set; }

    //[MaxLength (150)] -> alternative
    [Column(TypeName ="nvarchar(150)")]
    public string Name { get; set; }

    //float - half a day off
    public float NumberOfDays { get; set; }
}

