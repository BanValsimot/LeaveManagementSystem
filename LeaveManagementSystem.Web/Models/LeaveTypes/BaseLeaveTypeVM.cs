namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    //abstract class -> not instantiated anywhere
    public abstract class BaseLeaveTypeVM
    {
        //integer defaults to zero if there is no value
        public int Id { get; set; }
    }
}

