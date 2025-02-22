using ChitChat.Web.Extensions;
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

//// DB Context Injection ////
builder.Services.AddDbService(builder.Configuration);

//// Apply Authentication ////
builder.Services.AddAuthenticationService(builder.Configuration);

//// Change Model State Response Behavior ////
builder.Services.AddModelStateResponseService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//// Using Middlewares ////
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
