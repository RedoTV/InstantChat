namespace ChatService.Middlewares;

public class TaskCancellerationHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TaskCancellerationHandlingMiddleware> _logger;

    public TaskCancellerationHandlingMiddleware(
        RequestDelegate next,
        ILogger<TaskCancellerationHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception _) when (_ is OperationCanceledException or TaskCanceledException)
        {
            _logger.LogInformation("Task cancelled");
        }
    }
}
