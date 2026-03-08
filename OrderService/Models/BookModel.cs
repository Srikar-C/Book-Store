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
    public int Quantity {get; set;}
    public int SoldOut {get; set;} = 0;
    public int Count {get; set;} = 0;
    public bool Selected { get; set; } = false;
}