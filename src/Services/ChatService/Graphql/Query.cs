using ChatService.Models;
using ChatService.Services.Interfaces;

namespace ChatService.Graphql;

public class Query
{
    public async Task<ChatRoom?> GetChatRoomAsync(
        [Service] IChatRoomService chatRoomService,
        int chatRoomId,
        CancellationToken cancellationToken)
    {
        return await chatRoomService.GetChatRoomAsync(chatRoomId, cancellationToken); ;
    }

    [UseSorting]
    public async Task<IEnumerable<ChatRoom>> GetChatRoomsAsync(
        [Service] IChatRoomService chatRoomService,
        CancellationToken cancellationToken)
    {
        return await chatRoomService.GetAllChatRoomsAsync(cancellationToken); ;
    }
}