using ChatService.Models;
using ChatService.Services.Interfaces;
using HotChocolate;
using HotChocolate.Subscriptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Graphql
{
    public class Mutation
    {
        public async Task<ChatRoom> AddChatRoom(
            [Service] IChatRoomService chatRoomService,
            string name,
            [Service] ITopicEventSender sender,
            CancellationToken cancellationToken)
        {
            try
            {
                ChatRoom chatRoom = await chatRoomService.CreateChatRoomAsync(name, cancellationToken);
                await sender.SendAsync(nameof(AddChatRoom), chatRoom, cancellationToken);
                return chatRoom;
            }
            catch
            {
                return null!;
            }
        }

        public async Task<bool> DeleteChatRoom(
            [Service] IChatRoomService chatRoomService,
            int chatRoomId,
            CancellationToken cancellationToken)
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

        public async Task<UserChat> AddUserToChat(
            [Service] IUserChatService userChatService,
            Guid userId,
            int chatRoomId,
            CancellationToken cancellationToken)
        {
            try
            {
                UserChat userChat = await userChatService.AddUserToChat(userId, chatRoomId, cancellationToken);
                return userChat;
            }
            catch
            {
                return null!;
            }
        }
    }
}
