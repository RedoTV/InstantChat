using System.ComponentModel.DataAnnotations;

namespace ChatService.Models;

public class UserChat
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;
}