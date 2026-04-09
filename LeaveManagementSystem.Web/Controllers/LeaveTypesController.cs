using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using LeaveManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Web.Controllers;

public class LeaveTypesController(ILeaveTypeServices _leaveTypeServices) : Controller
{
    private const string NameExistsValidationMessage =
        "This leave type already exists in the Database";

    //GET: LeaveTypes -> LeaveTypeReadOnlyVM
    public async Task<IActionResult> Index()
    {
        var viewData = await _leaveTypeServices.GetAllAsync();

        //return the View Model to the View
        return View(viewData);
    }

    // GET: LeaveTypes/Details/5 -> LeaveTypeReadOnlyVM
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

    // POST: LeaveTypes/Create 
    // LeaveTypeCreateVM -> LeaveType
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
    {
        //Adding custom validation and model state error
        if(await _leaveTypeServices.CheckIfLeaveTypeNameExistsAsync(leaveTypeCreate.Name))
        {
            ModelState.AddModelError(nameof(leaveTypeCreate.Name),
                NameExistsValidationMessage);
        };

        if (ModelState.IsValid)
        {
            await _leaveTypeServices.CreateAsync(leaveTypeCreate);
            return RedirectToAction(nameof(Index));
        }
        //not valid
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

        var viewData = await _leaveTypeServices.GetAsync<LeaveTypeEditVM>(id.Value);
        
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
        if (await _leaveTypeServices.CheckIfLeaveTypeNameExistsForEditAsync(leaveTypeEdit))
        {
            ModelState.AddModelError(nameof(leaveTypeEdit.Name),
                NameExistsValidationMessage);
        };

        if (ModelState.IsValid)
        {
            try
            {
                await _leaveTypeServices.EditAsync(leaveTypeEdit);
            }
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
            return RedirectToAction(nameof(Index));
        }
        return View(leaveTypeEdit);
    }

    // GET: LeaveTypes/Delete/5
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
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _leaveTypeServices.RemoveAsync(id);

        return RedirectToAction(nameof(Index));
    }
}
