namespace ChatService.Models.Dtos;

public class UserJoinedPayload
{
    public int ChatRoomId { get; set; }
    public Guid UserId { get; set; }

    public UserJoinedPayload(int chatRoomId, Guid userId)
    {
        ChatRoomId = chatRoomId;
        UserId = userId;
    }
}