using ChatService.Data;
using ChatService.Graphql;
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

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

// Database migrate
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    dbContext.Database.Migrate();
}

app.MapGraphQL("/chat");

app.Run();