using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class BookModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id {get; set;}
    public string Title { get; set; }
    public string Author { get; set; }
    public string Url { get; set; }
    public decimal Price { get; set; }
    public bool Selected { get; set; } = false;
}