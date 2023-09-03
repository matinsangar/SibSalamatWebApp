using Microsoft.AspNetCore.Mvc;
using SibSalamat.Data;
using SibSalamat.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;


namespace SibSalamat.Controllers;

public class Account : Controller
{
    private readonly MongoDbContext _mongoDbContext;

    public Account(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    public IActionResult SignUp()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult userSignUp()
    {
        return View();
    }

    public IActionResult userLogin()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(Admin admin)
    {
        if (ModelState.IsValid)
        {
            await _mongoDbContext.RegisterAdminAsync(admin.Name, admin.Password, admin.Email, admin.NezamPezeshki);
            return RedirectToAction("Login");
        }
        foreach (var modelValue in ModelState.Values)
        {
            foreach (var error in modelValue.Errors)
            {
                // Log or debug the error messages
                var errorMessage = error.ErrorMessage;
                Console.WriteLine($"Validation Error: {errorMessage}");
            }
        }

        TempData["ErrorMessage"] = "اطلاعات داده شده معتبر نمی باشد";
        return RedirectToAction("SignUp");
        // await _mongoDbContext.RegisterAdminAsync(admin.Name, admin.Password, admin.Email, admin.NezamPezeshki);
        // return RedirectToAction("Login");
    }
}