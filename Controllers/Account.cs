using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SibSalamat.Data;
using SibSalamat.Models;
using MongoDB.Driver;
using SibSalamat.Views.Account;

namespace SibSalamat.Controllers;

public class Account : Controller
{
    private readonly MongoDbContext _mongoDbContext;
    private readonly IWebHostEnvironment _environment;
    private static string savedName;
    private static double savedTotalAmount;

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
            await _mongoDbContext.RegisterAdminAsync(admin.Name, admin.Password, admin.Email, admin.NezamPezeshki,
                admin.City);
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
            await _mongoDbContext.RegisterUserAsync(user.Name, user.Password, user.Email, user.NationalCode, user.City);
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
            savedName = user.Name;
            return RedirectToAction("userPanel", "Account");
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

        Console.WriteLine($"Adding product '{productName}' to favorites for user '{savedName}'...");
        bool result = await _mongoDbContext.AddToFavAsync(savedName, productName);

        if (result)
        {
            Console.WriteLine($"Product '{productName}' added to favorites for user '{savedName}'.");
            return Json(new { success = true });
        }

        Console.WriteLine($"Failed to add product '{productName}' to favorites.");
        return Json(new { success = false, message = "Failed to add product to favorites." });
    }

    [HttpGet]
    public async Task<IActionResult> userPanel()
    {
        var userCredit = await _mongoDbContext.getUserCredit(savedName);
        var viewModel = new UserPanelViewModel
        {
            Credit = userCredit
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> BuyPill(string productName, string userName, int count, double productPrice)
    {
        var user = await _mongoDbContext.getUserByName(savedName);
        var pill = await _mongoDbContext.getPillInfo(productName);

        if (user != null && pill != null)
        {
            var sell = new Sell(productName, pill.Price, count, pill.Provider, true, user.Name);
            await _mongoDbContext.SellsHistrory.InsertOneAsync(sell);
            await _mongoDbContext.UpdatePillCount(productName, count);
            user.BuyRoller.Add(sell);
            await _mongoDbContext.UpdateUser(user);
            return Json(new { success = true });
        }

        return Json(new { success = false, message = "User not found." });
    }

    public async Task<IActionResult> Payment()
    {
        var userSaleHistory = await _mongoDbContext.getSellInfoByUserName(savedName);
        var model = new PaymentViewModel()
        {
            SalesHistory = userSaleHistory,
            UserName = savedName
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeductUserCredit(double totalAmount)
    {
        try
        {
            savedTotalAmount = totalAmount;
            await _mongoDbContext.UpdateUserCredit(savedName, totalAmount);
            return Json(new { success = true, redirectUrl = Url.Action("PaymentSuccess") });
        }
        catch (Exception exp)
        {
            Console.WriteLine($"Error while Deducting: {exp.Message}");
            return Json(new { success = false, errorMessage = exp.Message });
        }
    }

    public IActionResult PaymentSuccess()
    {
        double totalAmountPaid = savedTotalAmount;
        var viewModel = new PaymentSuccessViewModel(totalAmountPaid);
        return View(viewModel);
    }

    public IActionResult userLogOut()
    {
        savedName = null;
        savedTotalAmount = double.NaN;
        return RedirectToAction("index", "Home");
    }
}