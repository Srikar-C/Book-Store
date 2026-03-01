using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class BooksModel
{
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? UserId{ get; set; }
    public string? BookName { get; set; }
    public string? BookAuthor {get; set;}
    public decimal BookPrice { get; set; }
    public string? BookDesc { get; set; }
    public bool BookSelect {get; set;}
}