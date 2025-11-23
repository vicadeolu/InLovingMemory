using InLovingMemory.Model;
using InLovingMemory.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("AppConfig:MongoDbSettings"));

builder.Services.AddSingleton<IMongoClient>(s =>
{
    var settings = builder.Configuration.GetSection("AppConfig:MongoDbSettings").Get<MongoDbSettings>();
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(s =>
{
    var settings = s.GetRequiredService<IConfiguration>()
                   .GetSection("AppConfig:MongoDbSettings").Get<MongoDbSettings>();
    var client = s.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});


builder.Services.AddScoped<MemorialTributeService>();
var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
    app.UseHttpsRedirection();

}

app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();