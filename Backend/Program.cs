using Backend.DAO;
using Backend.Services.Implementation;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IMojangApiClient, MojangApiClient>();
builder.Services.AddScoped<IMinecraftSkinService, MinecraftSkinService>();
builder.Services.AddScoped<IYouTubeThumbnailService, YouTubeThumbnailService>();

var connectionString = builder.Configuration["DB_CONNECTION_STRING"];
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();