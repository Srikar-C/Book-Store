using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class CartModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    public string? Id {get; set;}
    public string? UserId {get; set;}
    public List<BooksModel>? Carts {get; set;}
}