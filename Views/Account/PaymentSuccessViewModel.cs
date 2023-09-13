using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SibSalamat.Views.Account;

public class PaymentSuccessViewModel
{
    public PaymentSuccessViewModel(double totalAmount)
    {
        this.TotalAmount = totalAmount;
    }

    [BsonId] public ObjectId payId { get; set; }
    public double TotalAmount { get; set; }
}