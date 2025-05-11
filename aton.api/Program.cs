using aton.api.DI;
using aton.infrastructure.Data;
using aton.infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddSwaggerApi();
builder.Services.AddApplicationService();
builder.Services.AddOptions(builder.Configuration);

var app = builder.Build();

await SeedData.SeedDataAsync(app.Services);//Создание админа при условии созданной БД

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
