using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SibSalamat.Models;

public class Pill
{
    [BsonId] public ObjectId Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int ImageNumber { get; set; }
    public string? Description { get; set; } = "no info provided for this pill";

    public int? AvailableCount { get; set; } = 0;

    //Name and NezamPezeshki   
    public string Provider;
    public string PharmacyNumber;
}