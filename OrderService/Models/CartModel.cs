using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class CartModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    public string? Id { get; set; }
    public string? UserId{ get; set; }
    public string? BookName { get; set; }
    public string? BookAuthor {get; set;}
    public decimal BookPrice { get; set; }
    public string? BookDesc { get; set; }
}