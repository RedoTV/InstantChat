using ChatService.Data;
using ChatService.Graphql;
using ChatService.Services.Implementations;
using ChatService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string? dbConnection = builder.Configuration.GetConnectionString("DbConnection");

if (dbConnection == null)
{
    throw new System.Collections.Generic.KeyNotFoundException("DbConnection not found");
}

builder.Services.AddDbContext<ChatDbContext>(options =>
{
    options.UseSqlServer(dbConnection);
});

builder.Services.AddScoped<IChatRoomService, ChatRoomService>();

builder.Services.AddHttpClient<IUserChatService, UserChatService>(client =>
{
    //UserService url
    client.BaseAddress = new Uri("https://api.example.com/");
});

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

// Updating database to the latest migration version
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    dbContext.Database.Migrate();
}

app.MapGraphQL("/chat");

app.Run();