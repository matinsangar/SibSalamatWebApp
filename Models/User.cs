using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;


namespace SibSalamat.Models;

public class User
{
    [Required]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserID { get; set; }

    [Required] [MinLength(3)] public string Name { get; set; }
    [Required] [MinLength(5)] public string Password { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] [StringLength(4)] public string NationalCode { get; set; } //کدملی
}