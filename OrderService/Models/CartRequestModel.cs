using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class CartRequestModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    public string? Id {get; set;}
    public string? UserId {get; set;}
    public List<CartModel>? Carts {get; set;}
}