namespace SibSalamat.Models;

public class Sell
{
    public Sell(string productName, decimal price, int count, string pharmacyName, bool isValid, string userName)
    {
        ProductName = productName;
        Price = price;
        Count = count;
        PharmacyName = pharmacyName;
        IsValid = isValid;
        UserName = userName;
    }

    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; } = 0;
    public string PharmacyName { get; set; }
    public bool IsValid { get; set; } = false;
    public string UserName { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
}