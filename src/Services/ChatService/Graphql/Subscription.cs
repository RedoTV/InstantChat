using ChatService.Models;
using ChatService.Models.Dtos;

namespace ChatService.Graphql
{
    public class Subscription
    {
        [Subscribe]
        [Topic(nameof(Mutation.AddChatRoom))]
        public ChatRoom ChatCreated([EventMessage] ChatRoom chatRoom) => chatRoom;

        [Subscribe]
        [Topic("ChatActivity_{chatRoomId}")]
        public UserActivityPayload UserActivityInChat(int chatRoomId, [EventMessage] UserActivityPayload payload)
        {
            if (payload.ChatRoomId == chatRoomId)
            {
                return payload;
            }
            return null!;
        }
    }
}
