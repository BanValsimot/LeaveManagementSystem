using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.Services;

public interface ILeaveTypeServices
{
    Task<bool> CheckIfLeaveTypeNameExistsAsync(string name);
    Task<bool> CheckIfLeaveTypeNameExistsForEditAsync(LeaveTypeEditVM leaveTypeEdit);
    Task CreateAsync(LeaveTypeCreateVM model);
    Task EditAsync(LeaveTypeEditVM model);
    Task<List<LeaveTypeReadOnlyVM>> GetAllAsync();
    Task<T?> GetAsync<T>(int id) where T : class;
    bool LeaveTypeExists(int id);
    Task RemoveAsync(int id);
}