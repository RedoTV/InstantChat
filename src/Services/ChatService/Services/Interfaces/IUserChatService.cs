using ChatService.Models;
using ChatService.Models.Dtos;
using HotChocolate.Subscriptions;

namespace ChatService.Services.Interfaces;

public interface IUserChatService
{
    Task<UserChat> AddUserToChat(Guid userId, int chatRoomId, CancellationToken cancellationToken);
    Task<bool> RemoveUserFromChat(Guid userId, int chatRoomId, CancellationToken cancellationToken);
    Task<IEnumerable<Guid>> GetUsersIdInChatRoom(int chatRoomId, CancellationToken cancellationToken);
    Task<IEnumerable<ChatRoom>> GetUserChats(Guid userId, CancellationToken cancellationToken);
}
