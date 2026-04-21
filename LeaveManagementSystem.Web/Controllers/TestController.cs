using LeaveManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers;

//route address == Test
public class TestController : Controller
{
    //default View == Index
    public IActionResult Index()
    {
        //prepare Data
        var data = new TestViewModel
        {
            Name = "Student Tomo",
            DateOfBirth = new DateTime(1986, 11, 07)
        };
        return View(data);
    }
}
