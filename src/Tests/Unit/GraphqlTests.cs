using ChatService.Graphql;
using ChatService.Models;
using ChatService.Models.Dtos;
using ChatService.Models.Enums;
using ChatService.Services.Interfaces;
using HotChocolate.Subscriptions;
using Moq;

public class GraphqlTests
{
    // Mutation Tests
    [Fact]
    public async Task AddChatRoom_ShouldReturnChatRoom_WhenSuccessful()
    {
        // Arrange
        var chatRoomServiceMock = new Mock<IChatRoomService>();
        var senderMock = new Mock<ITopicEventSender>();
        var cancellationToken = CancellationToken.None;

        var chatRoom = new ChatRoom { Id = 1, Name = "Test Room", CreatedAt = DateTime.Now };
        chatRoomServiceMock
            .Setup(service => service.CreateChatRoomAsync(It.IsAny<string>(), cancellationToken))
            .ReturnsAsync(chatRoom);

        var mutation = new Mutation();

        // Act
        var result = await mutation.AddChatRoom(chatRoomServiceMock.Object, "Test Room", senderMock.Object, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(chatRoom.Name, result.Name);
        senderMock.Verify(sender => sender.SendAsync(nameof(Mutation.AddChatRoom), chatRoom, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteChatRoom_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        var chatRoomServiceMock = new Mock<IChatRoomService>();
        var cancellationToken = CancellationToken.None;

        chatRoomServiceMock
            .Setup(service => service.DeleteChatRoomAsync(It.IsAny<int>(), cancellationToken))
            .Returns(Task.CompletedTask);

        var mutation = new Mutation();

        // Act
        var result = await mutation.DeleteChatRoom(chatRoomServiceMock.Object, 1, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteChatRoom_ShouldReturnFalse_WhenExceptionThrown()
    {
        // Arrange
        var chatRoomServiceMock = new Mock<IChatRoomService>();
        var cancellationToken = CancellationToken.None;

        chatRoomServiceMock
            .Setup(service => service.DeleteChatRoomAsync(It.IsAny<int>(), cancellationToken))
            .ThrowsAsync(new Exception());

        var mutation = new Mutation();

        // Act
        var result = await mutation.DeleteChatRoom(chatRoomServiceMock.Object, 1, cancellationToken);

        // Assert
        Assert.False(result);
    }

    // Query Tests
    [Fact]
    public async Task GetChatRoomAsync_ShouldReturnChatRoom_WhenFound()
    {
        // Arrange
        var chatRoomServiceMock = new Mock<IChatRoomService>();
        var cancellationToken = CancellationToken.None;

        var chatRoom = new ChatRoom { Id = 1, Name = "Test Room", CreatedAt = DateTime.Now };
        chatRoomServiceMock
            .Setup(service => service.GetChatRoomAsync(It.IsAny<int>(), cancellationToken))
            .ReturnsAsync(chatRoom);

        var query = new Query();

        // Act
        var result = await query.GetChatRoomAsync(chatRoomServiceMock.Object, 1, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(chatRoom.Name, result.Name);
    }

    [Fact]
    public async Task GetChatRoomsAsync_ShouldReturnListOfChatRooms()
    {
        // Arrange
        var chatRoomServiceMock = new Mock<IChatRoomService>();
        var cancellationToken = CancellationToken.None;

        var chatRooms = new List<ChatRoom>
        {
            new ChatRoom { Id = 1, Name = "Room 1", CreatedAt = DateTime.Now },
            new ChatRoom { Id = 2, Name = "Room 2", CreatedAt = DateTime.Now }
        };
        chatRoomServiceMock
            .Setup(service => service.GetAllChatRoomsAsync(cancellationToken))
            .ReturnsAsync(chatRooms);

        var query = new Query();

        // Act
        var result = await query.GetChatRoomsAsync(chatRoomServiceMock.Object, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    // Subscription Tests
    [Fact]
    public void ChatCreated_ShouldReturnChatRoom()
    {
        // Arrange
        var subscription = new Subscription();
        var chatRoom = new ChatRoom { Id = 1, Name = "Test Room", CreatedAt = DateTime.Now };

        // Act
        var result = subscription.ChatCreated(chatRoom);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(chatRoom.Name, result.Name);
    }

    [Fact]
    public void UserActivityInChat_ShouldReturnPayload_WhenChatRoomIdMatches()
    {
        // Arrange
        var subscription = new Subscription();
        var payload = new UserActivityPayload(1, Guid.NewGuid(), UserActivityType.Joined);

        // Act
        var result = subscription.UserActivityInChat(1, payload);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(payload.ChatRoomId, result.ChatRoomId);
    }

    [Fact]
    public void UserActivityInChat_ShouldReturnNull_WhenChatRoomIdDoesNotMatch()
    {
        // Arrange
        var subscription = new Subscription();
        var payload = new UserActivityPayload(1, Guid.NewGuid(), UserActivityType.Joined);

        // Act
        var result = subscription.UserActivityInChat(2, payload);

        // Assert
        Assert.Null(result);
    }
}
