using ChatService.Models;
using ChatService.Models.Dtos;
using HotChocolate;
using HotChocolate.Types;

namespace ChatService.Graphql
{
    public class Subscription
    {
        [Subscribe]
        [Topic(nameof(Mutation.AddChatRoom))]
        public ChatRoom ChatCreated([EventMessage] ChatRoom chatRoom) => chatRoom;

        [Subscribe]
        [Topic("UserJoinedChat_{chatRoomId}")]
        public UserJoinedPayload UserJoinedToChat(int chatRoomId, [EventMessage] UserJoinedPayload payload)
        {
            if (payload.ChatRoomId == chatRoomId)
            {
                return payload;
            }
            return null!;
        }

    }
}
