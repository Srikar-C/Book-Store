using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class ForgotModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Cfn_Password{get; set; }
}