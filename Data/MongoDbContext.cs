using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using SibSalamat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing.Imaging;

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

    public IMongoCollection<PharmacyDrug> Pills
    {
        get { return _database.GetCollection<PharmacyDrug>("Pills"); }
    }


    public async Task RegisterUserAsync(string name, string password, string email, string national_code)
    {
        var user = new User
        {
            UserID = ObjectId.GenerateNewId().ToString(),
            Name = name,
            Password = password,
            Email = email,
            NationalCode = national_code
        };
        await Users.InsertOneAsync(user);
    }

    public async Task RegisterAdminAsync(string Name, string Password, string Email, string NezamPezeshki)
    {
        var admin = new Admin
        {
            ID = ObjectId.GenerateNewId().ToString(),
            Name = Name,
            Password = Password,
            Email = Email,
            NezamPezeshki = NezamPezeshki
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

    public async Task CreatePharmacyDrugAsync(PharmacyDrug pharmacyDrug)
    {
        await Pills.InsertOneAsync(pharmacyDrug);
    }

    public async Task<PharmacyDrug> GetPharmacyDrugAsync(string id)
    {
        return await Pills.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PharmacyDrug>> GetAllPharmacyDrugsAsync()
    {
        return await Pills.Find(_ => true).ToListAsync();
    }

    public async Task UpdatePharmacyDrugAsync(string id, PharmacyDrug pharmacyDrug)
    {
        await Pills.ReplaceOneAsync(x => x.Id == id, pharmacyDrug);
    }

    public async Task<bool> DeletePharmacyDrugAsync(string id)
    {
        var result = await Pills.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task AddImageToPharmacyDrugAsync(string id, byte[] imageBytes)
    {
        var filter = Builders<PharmacyDrug>.Filter.Eq("_id", ObjectId.Parse(id));
        var update = Builders<PharmacyDrug>.Update.Set("Image", imageBytes);
        await Pills.UpdateOneAsync(filter, update);
    }

    public async Task<byte[]> GetImageForPharmacyDrugAsync(string id)
    {
        var pharmacyDrug = await GetPharmacyDrugAsync(id);
        return pharmacyDrug?.Image;
    }
}