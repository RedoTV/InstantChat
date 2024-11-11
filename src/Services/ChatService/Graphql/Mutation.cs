using ChatService.Models;
using ChatService.Services.Interfaces;

namespace ChatService.Graphql;

public class Mutation
{
    public async Task<ChatRoom> AddChatRoom(
        [Service] IChatRoomService chatRoomService,
        string name,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await chatRoomService.CreateChatRoomAsync(name, cancellationToken);
        }
        catch
        {
            return null!;
        }
    }

    public async Task<bool> DeleteChatRoom(
        [Service] IChatRoomService chatRoomService,
        int chatRoomId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await chatRoomService.DeleteChatRoomAsync(chatRoomId, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
