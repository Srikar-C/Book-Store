public class ResponseModel
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public List<BookModel>? Books { get; set; }
    public List<OrderModel>? Orders {get; set;} 
}