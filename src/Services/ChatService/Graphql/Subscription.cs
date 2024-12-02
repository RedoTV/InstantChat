using ChatService.Models;
using HotChocolate;
using HotChocolate.Types;

namespace ChatService.Graphql
{
    public class Subscription
    {
        [Subscribe]
        [Topic(nameof(Mutation.AddChatRoom))]
        public ChatRoom ChatCreated([EventMessage] ChatRoom chatRoom) => chatRoom;
    }
}
