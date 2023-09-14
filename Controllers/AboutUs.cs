using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SibSalamat.Data;
using SibSalamat.Models;
using MongoDB.Driver;
using SibSalamat.Views.Account;


namespace SibSalamat.Controllers;

public class AboutUs:Controller
{
    public IActionResult AboutUsPage()
    {
        return View("AboutUs");
    }
}