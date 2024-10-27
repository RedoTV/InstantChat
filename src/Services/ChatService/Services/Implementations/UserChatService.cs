using ChatService.Models;
using ChatService.Models.Dtos;
using ChatService.Services.Interfaces;

namespace ChatService.Services.Implementations;

public class UserChatService : IUserChatService
{
    public Task AddUserToChat(int userId, int chatRoomId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ChatRoom>> GetUserChats(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUsersInChatRoom(int chatRoomId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveUserFromChat(int userId, int chatroomId)
    {
        throw new NotImplementedException();
    }
}
