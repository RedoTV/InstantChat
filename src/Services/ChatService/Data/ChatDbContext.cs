using ChatService.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<UserChat> UserChats { get; set; }

}
