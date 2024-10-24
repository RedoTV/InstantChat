namespace MessageService.Models;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }
    public int UserId { get; set; }
    public int ChatRoomId { get; set; }
}