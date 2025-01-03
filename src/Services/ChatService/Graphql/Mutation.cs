using ChatService.Models;
using ChatService.Models.Dtos;
using ChatService.Models.Enums;
using ChatService.Services.Interfaces;
using HotChocolate.Subscriptions;

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
            [Service] ITopicEventSender sender,
            CancellationToken cancellationToken)
        {
            try
            {
                UserChat userChat = await userChatService.AddUserToChat(userId, chatRoomId, cancellationToken);

                var payload = new UserActivityPayload(chatRoomId, userId, UserActivityType.Joined);
                string topic = $"ChatActivity_{chatRoomId}";
                await sender.SendAsync(topic, payload, cancellationToken);

                return userChat;
            }
            catch
            {
                return null!;
            }
        }

        public async Task<bool> RemoveUserFromChat(
            [Service] IUserChatService userChatService,
            Guid userId,
            int chatRoomId,
            [Service] ITopicEventSender sender,
            CancellationToken cancellationToken)
        {
            try
            {
                var userIsRemoved = await userChatService.RemoveUserFromChat(userId, chatRoomId, cancellationToken);

                if (userIsRemoved == false)
                    return false;

                var payload = new UserActivityPayload(chatRoomId, userId, UserActivityType.Left);
                string topic = $"ChatActivity_{chatRoomId}";
                await sender.SendAsync(topic, payload, cancellationToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
