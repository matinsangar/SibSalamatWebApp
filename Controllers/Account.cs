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
}