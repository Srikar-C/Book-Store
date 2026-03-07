using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class SessionModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string SessionId { get; set; } // Unique identifier for the session
    public string UserId { get; set; } // Reference to the user associated with the
    public string JWTToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiry { get; set; }

}