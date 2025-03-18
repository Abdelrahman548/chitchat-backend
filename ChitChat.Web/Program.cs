using ChitChat.Web.Extensions;
using ChitChat.Web.Hubs;
using ChitChat.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//// Swagger Authorize Button ////
builder.Services.AddSwaggerDocumentation();

//// Dependency Injection ////
builder.Services.AddApplicationService();

//// Configurations Loading ////
builder.Services.AddOptionsService(builder.Configuration);

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
var logger = loggerFactory.CreateLogger("Program");

//// DB Context Injection ////
builder.Services.AddDbService(builder.Configuration, logger);

//// Apply Authentication ////
builder.Services.AddAuthenticationService(builder.Configuration);

//// Change Model State Response Behavior ////
builder.Services.AddModelStateResponseService();

//// Change Model State Response Behavior ////
builder.Services.AddMemoryCache();

//// Add SignalR ////
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//// Using Middlewares ////
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseAuthorization();

app.MapControllers();

app.UseWebSockets();
app.UseCors("AllowAll");
app.MapHub<ChatHub>("/chatHub");

app.Run();
