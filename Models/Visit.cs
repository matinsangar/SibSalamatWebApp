using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;


namespace SibSalamat.Models;

public class Visit
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string visitID { get; set; } = string.Empty;

    public Admin Doctor { get; set; }
    public User Patient { get; set; }
    public DateTime time { get; set; }
    public string City { get; set; }
    public string Status { get; set; }
}