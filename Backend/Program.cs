using Backend.DAO;
using Backend.DAO.Repositories.Implementations;
using Backend.DAO.Repositories.Interfaces;
using Backend.Services.Implementation;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPackRepository, PackRepository>();
builder.Services.AddScoped<ICardBaseRepository, CardBaseRepository>();

builder.Services.AddHttpClient<IMojangApiClient, MojangApiClient>();
builder.Services.AddScoped<IMinecraftSkinService, MinecraftSkinService>();
builder.Services.AddScoped<IYouTubeThumbnailService, YouTubeThumbnailService>();
builder.Services.AddScoped<IFileStorageService, MinioFileStorageService>();
builder.Services.AddScoped<IPackService, PackService>();
builder.Services.AddScoped<ICardBaseService, CardBaseService>();

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