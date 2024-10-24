using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Models;

public class ChatRoom
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; }

    public ICollection<UserChat> UserChats { get; set; } = null!;
}