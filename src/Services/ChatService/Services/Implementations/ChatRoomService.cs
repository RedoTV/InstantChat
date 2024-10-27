using ChatService.Data;
using ChatService.Models;
using ChatService.Services.Interfaces;

namespace ChatService.Services.Implementations;

public class ChatRoomService : IChatRoomService
{
    public ChatRoomService(ChatDbContext _dbContext)
    {
        
    }
    public Task CreateChatRoom(string name)
    {
        throw new NotImplementedException();
    }

    public Task DeleteChatRoom(int chatRoomId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ChatRoom>> GetAllChatRooms()
    {
        throw new NotImplementedException();
    }

    public Task<ChatRoom> GetChatRoom(int chatRoomId)
    {
        throw new NotImplementedException();
    }
}
