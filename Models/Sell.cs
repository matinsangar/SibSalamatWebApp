using MongoDB.Bson;

namespace SibSalamat.Models;

public class Sell
{
    public Sell(string productName, double price, int count, string pharmacyName, bool isValid, string userName)
    {
        ProductName = productName;
        Price = price;
        Count = count;
        PharmacyName = pharmacyName;
        IsValid = isValid;
        UserName = userName;
    }

    public ObjectId Id { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
    public int Count { get; set; } = 0;
    public string PharmacyName { get; set; }
    public bool IsValid { get; set; } = false;
    public string UserName { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
}