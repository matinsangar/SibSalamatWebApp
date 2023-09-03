using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace SibSalamat.Models;

public class Admin
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ID { get; set; } = string.Empty;

    [Required(ErrorMessage = "نام کاربری الزامی است.")]
    [MinLength(3)]
    public string Name { get; set; }

    [Required(ErrorMessage = "رمز عبور الزامی است.")]
    [MinLength(3)]
    public string Password { get; set; }

    [Required(ErrorMessage = "ایمیل الزامی است.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "نظام پزشکی الزامی است.")]
    public string NezamPezeshki { get; set; }
}