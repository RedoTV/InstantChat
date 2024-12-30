using ChatService.Models;
using ChatService.Models.Dtos;
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

                // Publish the event to the specific chat room topic
                var payload = new UserJoinedPayload(chatRoomId, userId);
                string topic = $"UserJoinedChat_{chatRoomId}";
                await sender.SendAsync(topic, payload, cancellationToken);

                return userChat;
            }
            catch
            {
                return null!;
            }
        }
    }
}
