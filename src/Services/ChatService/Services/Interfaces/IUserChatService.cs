using ChatService.Models;
using ChatService.Models.Dtos;

namespace ChatService.Services.Interfaces;

public interface IUserChatService
{
    Task AddUserToChat(int userId, int chatRoomId);
    Task RemoveUserFromChat(int userId, int chatroomId);
    Task<User> GetUsersInChatRoom(int chatRoomId);
    Task<IEnumerable<ChatRoom>> GetUserChats(int userId);
}
