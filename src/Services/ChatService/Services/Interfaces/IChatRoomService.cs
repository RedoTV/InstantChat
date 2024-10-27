using ChatService.Models;

namespace ChatService.Services.Interfaces;

public interface IChatRoomService
{
    Task CreateChatRoom(string name);
    Task<ChatRoom> GetChatRoom(int chatRoomId);
    Task<IEnumerable<ChatRoom>> GetAllChatRooms();
    Task DeleteChatRoom(int chatRoomId);
}
