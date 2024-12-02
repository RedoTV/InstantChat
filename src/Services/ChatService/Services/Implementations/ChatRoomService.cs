using AutoMapper;
using ChatService.Data;
using ChatService.Models;
using ChatService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Services.Implementations;

public class ChatRoomService : IChatRoomService
{
    public ChatDbContext _dbContext { get; }

    public ChatRoomService(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ChatRoom> CreateChatRoomAsync(string name, CancellationToken cancellationToken)
    {
        if (name is null || name is "")
            throw new ArgumentException("name is empty");

        var chatRoom = new ChatRoom()
        {
            Name = name,
            CreatedAt = DateTime.Now
        };
        await _dbContext.ChatRooms.AddAsync(chatRoom, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        chatRoom = await _dbContext.ChatRooms
            .Where(r =>
                r.Name == chatRoom.Name &&
                r.CreatedAt == chatRoom.CreatedAt
            )
            .FirstAsync(cancellationToken);


        return chatRoom;
    }

    public async Task DeleteChatRoomAsync(int chatRoomId, CancellationToken cancellationToken)
    {
        var chatRoom = await _dbContext.ChatRooms.FindAsync(chatRoomId, cancellationToken);
        if (chatRoom is null)
            throw new ArgumentException($"charRoom with id={chatRoomId} not found");

        _dbContext.Remove(chatRoom);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ChatRoom>> GetAllChatRoomsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.ChatRooms
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task<ChatRoom?> GetChatRoomAsync(int chatRoomId, CancellationToken cancellationToken)
    {
        return await _dbContext.ChatRooms
            .FindAsync(chatRoomId, cancellationToken);
    }
}
