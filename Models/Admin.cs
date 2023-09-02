using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace SibSalamat.Models;

public class Admin
{
    [Required]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string AdminID { get; set; }

    [Required] [MinLength(3)] public string Name { get; set; }
    [Required] [MinLength(5)] public string Password { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
}