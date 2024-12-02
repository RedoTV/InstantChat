using ChatService.Data;
using ChatService.Models;
using ChatService.Models.Dtos;
using ChatService.Services.Interfaces;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Services.Implementations;

public class UserChatService : IUserChatService
{
    public ChatDbContext _dbContext { get; }

    public UserChatService(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserChat> AddUserToChat(Guid userId, int chatRoomId, CancellationToken cancellationToken)
    {
        var userChat = new UserChat
        {
            UserId = userId,
            ChatRoomId = chatRoomId
        };

        await _dbContext.UserChats.AddAsync(userChat, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return userChat;
    }

    public async Task<IEnumerable<ChatRoom>> GetUserChats(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.UserChats
            .Where(uc => uc.UserId == userId)
            .Select(uc => uc.ChatRoom)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Guid>> GetUsersIdInChatRoom(int chatRoomId, CancellationToken cancellationToken)
    {
        return await _dbContext.UserChats
            .Where(uc => uc.ChatRoomId == chatRoomId)
            .Select(uc => uc.UserId)
            .ToListAsync(cancellationToken);
    }

    public async Task RemoveUserFromChat(Guid userId, int chatRoomId, CancellationToken cancellationToken)
    {
        var userChat = await _dbContext.UserChats
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ChatRoomId == chatRoomId, cancellationToken);

        if (userChat != null)
        {
            _dbContext.UserChats.Remove(userChat);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
