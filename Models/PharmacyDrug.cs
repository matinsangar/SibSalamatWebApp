using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace SibSalamat.Models;

public class PharmacyDrug
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string ProductNumber { get; set; }

    [Required] public decimal Fee { get; set; }

    [Required] public int Count { get; set; }

    [BsonIgnoreIfNull] public byte[] Image { get; set; }

    [BsonIgnore] public string ImageBase64 => Image != null ? Convert.ToBase64String(Image) : null;
}