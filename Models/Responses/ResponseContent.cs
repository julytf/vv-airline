namespace vv_airline.Models.Responses;
public class ResponseContent
{
    public string? Title { get; set; }
    public string? Status { get; set; } = "Success";
    public object? Data { get; set; }
}