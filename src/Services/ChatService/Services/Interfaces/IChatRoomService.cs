using ChatService.Models;

namespace ChatService.Services.Interfaces;

public interface IChatRoomService
{
    Task<ChatRoom> CreateChatRoomAsync(string name, CancellationToken cancellationToken);
    Task<ChatRoom?> GetChatRoomAsync(int chatRoomId, CancellationToken cancellationToken);
    Task<IEnumerable<ChatRoom>> GetAllChatRoomsAsync(CancellationToken cancellationToken);
    Task DeleteChatRoomAsync(int chatRoomId, CancellationToken cancellationToken);
}
