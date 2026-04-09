using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services;

//use Primary Constructor
public class LeaveTypeServices(ApplicationDbContext _context, IMapper _mapper) : ILeaveTypeServices
{

    //GET all LeaveTypesReadOnlyVM
    public async Task<List<LeaveTypeReadOnlyVM>> GetAllAsync()
    {
        //var data = SELECT * FROM LeaveTypes -> GET Data from Database
        var data = await _context.LeaveTypes.ToListAsync();
        //convert the List of Data Models into a List of View Models - use AutoMapper
        var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);
        return viewData;
    }

    //GET specific Record based on ID
    //generic async method -> T = nullable
    public async Task<T?> GetAsync<T>(int id) where T : class
    {
        var data = await _context.LeaveTypes.FirstOrDefaultAsync(
            record => record.Id == id);

        if (data == null)
        {
            return null;
        }

        var viewData = _mapper.Map<T>(data);

        return viewData;
    }

    //GET & DELETE a Record
    public async Task RemoveAsync(int id)
    {
        var data = await _context.LeaveTypes.FirstOrDefaultAsync(
            record => record.Id == id);

        if (data != null)
        {
            _context.Remove(data);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EditAsync(LeaveTypeEditVM model)
    {
        var leaveType = _mapper.Map<LeaveType>(model);
        _context.Update(leaveType);
        await _context.SaveChangesAsync();
    }

    public async Task CreateAsync(LeaveTypeCreateVM model)
    {
        var leaveType = _mapper.Map<LeaveType>(model);
        _context.Add(leaveType);
        await _context.SaveChangesAsync();
    }

    public bool LeaveTypeExists(int id)
    {
        return _context.LeaveTypes.Any(e => e.Id == id);
    }

    public async Task<bool> CheckIfLeaveTypeNameExistsAsync(string name)
    {
        var lowercaseName = name.ToLower();
        return await _context.LeaveTypes.AnyAsync(
            record => record.Name.ToLower().Equals(lowercaseName));
    }

    public async Task<bool> CheckIfLeaveTypeNameExistsForEditAsync(LeaveTypeEditVM leaveTypeEdit)
    {
        var lowercaseName = leaveTypeEdit.Name.ToLower();
        return await _context.LeaveTypes.AnyAsync(
            record => record.Name.ToLower().Equals(lowercaseName) &&
            record.Id != leaveTypeEdit.Id);
    }
}


