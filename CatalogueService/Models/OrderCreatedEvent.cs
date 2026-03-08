public class OrderCreatedEvent
{
    public string UserId { get; set; }
    public List<BookModel> Books { get; set; }
    public DateTime CreatedAt { get; set; }
}