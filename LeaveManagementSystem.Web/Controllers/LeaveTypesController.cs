using LeaveManagementSystem.Web.Models.LeaveTypes;
using LeaveManagementSystem.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Controllers;

//Database UI Controller -> scaffolded (with Views)
//Primary Constructor used

[Authorize(Roles = Roles.Administrator)]
public class LeaveTypesController(ILeaveTypeServices _leaveTypeServices) : Controller
{
    private const string NameExistsValidationMessage =
        "This leave type already exists in the Database";

    //GET: LeaveTypes -> LeaveTypeReadOnlyVM
    public async Task<IActionResult> Index()
    {
        //Get a list of LeaveTypes -> overview of Data in the Database
        var viewData = await _leaveTypeServices.GetAllAsync();

        //return the View Model to the View
        return View(viewData);
    }

    // GET: LeaveTypes/Details/5 -> LeaveTypeReadOnlyVM (get one record)
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        //Specify data that method retrieves -> ID is not null here
        var viewData = 
            await _leaveTypeServices.GetAsync<LeaveTypeReadOnlyVM>(id.Value);

        if (viewData == null)
        {
            return NotFound();
        }
        return View(viewData);
    }

    // GET: LeaveTypes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: LeaveTypes/Create -> Bind Data to LeaveTypeCreateVM object
    // transform LeaveTypeCreateVM -> LeaveType
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
    {
        //Adding custom validation and model state error
        if(await _leaveTypeServices.CheckIfLeaveTypeNameExistsAsync(leaveTypeCreate.Name))
        {
            //add custom Error to the Model State    
            ModelState.AddModelError(nameof(leaveTypeCreate.Name),
                NameExistsValidationMessage);
        };

        //ModelState stores the state of data submitted from a form,
        //including values and validation errors
        if (ModelState.IsValid)
        {
            await _leaveTypeServices.CreateAsync(leaveTypeCreate);
            return RedirectToAction(nameof(Index));
        }

        //Data not valid -> return to a Create page (errors will appear in Browser)
        return View(leaveTypeCreate);
    }

    // GET: LeaveTypes/Edit/5
    // LeaveType -> LeaveTypeEditVM
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var viewData =
            await _leaveTypeServices.GetAsync<LeaveTypeEditVM>(id.Value);
        
        if (viewData == null)
        {
            return NotFound();
        }

        return View(viewData);
    }

    // POST: LeaveTypes/Edit/5
    // LeaveTypeEditVM -> LeaveType
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
    {
        if (id != leaveTypeEdit.Id)
        {
            return NotFound();
        }

        //Adding custom validation and model state error
        //Is there ANY other record with the same Name but a different Id? ->
        //Duplicate exists → return true → add ModelState error
        if (await _leaveTypeServices.CheckIfLeaveTypeNameExistsForEditAsync(
            leaveTypeEdit))
        {
            ModelState.AddModelError(nameof(leaveTypeEdit.Name),
                NameExistsValidationMessage);
        };

        //ModelState stores the state of data submitted from a form,
        //including values and validation errors
        if (ModelState.IsValid)
        {
            try
            {
                await _leaveTypeServices.EditAsync(leaveTypeEdit);
            }
            // DbUpdateConcurrencyException occurs when multiple users try to update or delete
            // the same database record at the same time (concurrency conflict).
            // It means the data in the database has changed since it was originally loaded,
            // so the current operation cannot be completed safely.
            catch (DbUpdateConcurrencyException)
            {
                if (!_leaveTypeServices.LeaveTypeExists(leaveTypeEdit.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //redirect to Main View
            return RedirectToAction(nameof(Index));
        }
        //return to Edit View (show errors)
        return View(leaveTypeEdit);
    }

    // GET: LeaveTypes/Delete/5
    // Return LeaveTypeReadOnlyVM (via ID)
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var viewData = 
            await _leaveTypeServices.GetAsync<LeaveTypeReadOnlyVM>(id.Value);
        
        if (viewData == null)
        {
            return NotFound();
        }

        return View(viewData);
    }

    // POST: LeaveTypes/Delete/5
    //ActionName("Delete") -> allows overloading when 2 methods have
    //the same signature but should have the same name
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _leaveTypeServices.RemoveAsync(id);

        return RedirectToAction(nameof(Index));
    }
}
