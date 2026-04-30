using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services;

//The service layer handles business logic and coordinates data access
//using Entity Framework Core.
//It performs CRUD operations, applies validation rules, and ensures data consistency
//(e.g., preventing duplicates). 
//EF Core acts as a repository and automatically uses parameterized queries
//to protect against SQL injection.

//use Primary Constructor
//_contex = Database Connection
public class LeaveTypeServices(
    ApplicationDbContext _context, IMapper _mapper) : ILeaveTypeServices
{

    //GET all LeaveTypeReadOnlyVM (Index View)
    public async Task<List<LeaveTypeReadOnlyVM>> GetAllAsync()
    {
        //var data = SELECT * FROM LeaveTypes -> GET Data from Database
        //LeaveTypes -> name of the table with required Data
        //ToListAsync -> materialize the Query (List of LeaveTypes objects)
        var data = await _context.LeaveTypes.ToListAsync();
        //convert the List of Data Models into a List of View Models - use AutoMapper
        var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);
        return viewData;
    }

    //GET specific Record based on ID
    //generic async method -> T = nullable
    //returns LeaveTypeReadOnlyVM (Details View) & LeaveTypeEditVM (Edit View)
    public async Task<T?> GetAsync<T>(int id) where T : class
    {
        //_context -> Database connection
        //LeaveTypes -> table
        //SELECT * FROM LeaveTypes WHERE Id == @id
        //Parameterization -> Key for preventig SQL injection attacks 
        var data = await _context.LeaveTypes.FirstOrDefaultAsync(
            record => record.Id == id);

        //FirstOrDefaultAsync -> returns null if nothing found (T? nullable)
        if (data == null)
        {
            return null;
        }

        //convert to LeaveTypeReadOnlyVM or LeaveTypeEditVM
        var viewData = _mapper.Map<T>(data);
        return viewData;
    }

    //LeaveTypeCreateVM -> LeaveType & add a Record
    public async Task CreateAsync(LeaveTypeCreateVM model)
    {
        var leaveType = _mapper.Map<LeaveType>(model);
        //_context -> Database connection
        _context.Add(leaveType);
        //save changes to a Database
        await _context.SaveChangesAsync();
    }

    //LeaveTypeEditVM -> LeaveType & update Table
    public async Task EditAsync(LeaveTypeEditVM model)
    {
        var leaveType = _mapper.Map<LeaveType>(model);
        _context.Update(leaveType);
        await _context.SaveChangesAsync();
    }

    //GET & DELETE a Record -> Void (nothing returned)
    public async Task RemoveAsync(int id)
    {
        var data = await _context.LeaveTypes.FirstOrDefaultAsync(
            record => record.Id == id);

        if (data != null)
        {
            _context.Remove(data);
            await _context.SaveChangesAsync();
        }

        //do nothing
    }

    //Helper Functions
    public bool LeaveTypeExists(int id)
    {
        return _context.LeaveTypes.Any(record => record.Id == id);
    }

    public async Task<bool> CheckIfLeaveTypeNameExistsAsync(string name)
    {
        var lowercaseName = name.ToLower();
        return await _context.LeaveTypes.AnyAsync(
            //StringComparison.InvariantCultureIgnoreCase -> cannot be translated to SQL
            record => record.Name.ToLower().Equals(lowercaseName));
    }

    //Find another record with same Name BUT different Id
    public async Task<bool> CheckIfLeaveTypeNameExistsForEditAsync(
        LeaveTypeEditVM leaveTypeEdit)
    {
        var lowercaseName = leaveTypeEdit.Name.ToLower();
        //cannot Edit leave type with the same Name and different Id
        return await _context.LeaveTypes.AnyAsync(
            record => record.Name.ToLower().Equals(lowercaseName) &&
            record.Id != leaveTypeEdit.Id);
    }
}


