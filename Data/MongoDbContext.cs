using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using SibSalamat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SibSalamat.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var mongoClinet = new MongoClient(connectionString);
        _database = mongoClinet.GetDatabase("SibSalamatApp");
    }

    public IMongoCollection<Admin> Admins
    {
        get { return _database.GetCollection<Admin>("Admins"); }
    }

    public IMongoCollection<User> Users
    {
        get { return _database.GetCollection<User>("Users"); }
    }

    public IMongoCollection<Pill> Pills => _database.GetCollection<Pill>("Pills");

    public async Task RegisterUserAsync(string name, string password, string email, string nationalCode, string city)
    {
        var user = new User
        {
            UserID = ObjectId.GenerateNewId().ToString(),
            Name = name,
            Password = password,
            Email = email,
            NationalCode = nationalCode,
            City = city
        };
        await Users.InsertOneAsync(user);
    }

    public async Task RegisterAdminAsync(string name, string password, string email, string nezamPezeshki, string city)
    {
        var admin = new Admin
        {
            ID = ObjectId.GenerateNewId().ToString(),
            Name = name,
            Password = password,
            Email = email,
            NezamPezeshki = nezamPezeshki,
            City = city
        };
        await Admins.InsertOneAsync(admin);
    }

    public async Task<bool> VerifyAdminLogin(string name, string password, string nezam_pezeshki)
    {
        var admin = await Admins.Find(a => a.Name == name).FirstOrDefaultAsync();
        if (admin != null && admin.Password == password && admin.NezamPezeshki == nezam_pezeshki)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> VerifyUserLogin(string name, string password, string national_code)
    {
        var user = await Users.Find(u => u.Name == name).FirstOrDefaultAsync();
        if (user != null && user.Password == password && user.NationalCode == national_code)
        {
            return true;
        }

        return false;
    }

    public async Task CreatePharmacyDrugAsync(Pill pill)
    {
        await Pills.InsertOneAsync(pill);
    }

    public async Task<List<string>> GetUserFavAsync(string userID)
    {
        var user = await Users.Find(u => u.UserID == userID).FirstOrDefaultAsync();
        if (user != null && user.Favorites != null)
        {
            return user.Favorites;
        }

        return new List<string>();
    }

    public async Task<bool> AddToFavAsync(string userName, string productName)
    {
        var user = await Users.Find(u => u.Name == userName).FirstOrDefaultAsync();
        if (user != null)
        {
            if (user.Favorites == null)
            {
                user.Favorites = new List<string>();
            }

            if (!user.Favorites.Contains(productName))
            {
                user.Favorites.Add(productName);

                //Update mongoDB 
                var updatedUser = await Users.ReplaceOneAsync(u => u.Name == userName, user);
                return updatedUser.IsAcknowledged && updatedUser.ModifiedCount > 0;
            }
        }

        return false;
    }

    public async Task<double> getUserCredit(string username)
    {
        var user = await Users.Find(u => u.Name == username).FirstOrDefaultAsync();
        if (user != null)
        {
            return user.Credit;
        }

        return 20;
    }

    public async Task<Pill> getPillInfo(string name)
    {
        var pill = await Pills.Find(p => p.Name == name).FirstOrDefaultAsync();
        return pill;
    }

    public async Task<User> getUserByName(string name)
    {
        var user = await Users.Find(u => u.Name == name).FirstOrDefaultAsync();
        return user;
    }

    public async Task<bool> UpdateUser(User user)
    {
        try
        {
            var result = await Users.ReplaceOneAsync(x => x.Name == user.Name, user);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user: {ex.Message}");
            return false;
        }
    }
}