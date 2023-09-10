using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SibSalamat.Data;
using SibSalamat.Models;
using MongoDB.Driver;

namespace SibSalamat.Controllers;

public class Account : Controller
{
    private readonly MongoDbContext _mongoDbContext;
    private readonly IWebHostEnvironment _environment;
    private static string savedName;

    public Account(MongoDbContext mongoDbContext, IWebHostEnvironment environment)
    {
        _mongoDbContext = mongoDbContext;
        _environment = environment;
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
    }

    [HttpPost]
    public async Task<IActionResult> userSignUp(User user)
    {
        if (ModelState.IsValid)
        {
            await _mongoDbContext.RegisterUserAsync(user.Name, user.Password, user.Email, user.NationalCode);
            return RedirectToAction("userLogin");
        }

        TempData["ErrorMessage"] = "اطلاعات داده شده معتبر نمی باشد";
        return RedirectToAction("userSignUp");
    }

    [HttpPost]
    public async Task<IActionResult> Login(Admin admin)
    {
        var isLoginValid = await _mongoDbContext.VerifyAdminLogin(admin.Name, admin.Password, admin.NezamPezeshki);
        if (isLoginValid)
        {
            return RedirectToAction("Index", "Home");
        }

        TempData["ErrorMessage"] = "Invalid username or password.";
        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> userLogin(User user)
    {
        var isLoginValid = await _mongoDbContext.VerifyUserLogin(user.Name, user.Password, user.NationalCode);
        if (isLoginValid)
        {
            HttpContext.Session.SetString("UserId", user.UserID);
            TempData["UserName"] = user.Name;
            savedName = user.Name;
            return RedirectToAction("DisplayAllPills", "Account");
        }

        TempData["ErrorMessage"] = "Invalid username or password.";
        return RedirectToAction("userLogin");
    }

    [HttpGet]
    public IActionResult AddPill()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddPill(Pill pill)
    {
        if (ModelState.IsValid)
        {
            if (pill.ImageNumber < 1 || pill.ImageNumber > 6)
            {
                ModelState.AddModelError("ImageNumber", "Invalid Image Number");
                return View(pill);
            }

            _mongoDbContext.CreatePharmacyDrugAsync(pill);
            return RedirectToAction("Index", "Home");
        }

        return View(pill);
    }

    [HttpGet]
    public async Task<IActionResult> DisplayAllPills()
    {
        var pills = await _mongoDbContext.Pills.Find(_ => true).ToListAsync();
        return View(pills);
    }

    [HttpPost]
    public async Task<IActionResult> AddToFavorites(string productName, string userName2)
    {
        //savedname is loggedin client username  
        
        string userName = TempData["UserName"] as string;
        Console.WriteLine($"Adding product '{productName}' to favorites for user '{savedName}'...");
        bool result = await _mongoDbContext.AddToFavAsync(savedName, productName);

        if (result)
        {
            Console.WriteLine($"Product '{productName}' added to favorites for user '{userName}'.");
            return Json(new { success = true });
        }

        Console.WriteLine($"Failed to add product '{productName}' to favorites.");
        return Json(new { success = false, message = "Failed to add product to favorites." });
    }
}