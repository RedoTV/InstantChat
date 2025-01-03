using ChatService.Models.Enums;

namespace ChatService.Models.Dtos;

public class UserActivityPayload
{
    public int ChatRoomId { get; }
    public Guid UserId { get; }
    public UserActivityType ActivityType { get; }

    public UserActivityPayload(int chatRoomId, Guid userId, UserActivityType activityType)
    {
        ChatRoomId = chatRoomId;
        UserId = userId;
        ActivityType = activityType;
    }
}