using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class LoginResponseModel
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; }
    
}