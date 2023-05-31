using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

if(builder.Environment.IsProduction()){
    builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}
else{
    builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseInMemoryDatabase("InMem"));
}



// builder.Services.AddDbContext<AppDbContext>(opt => 
// opt.UseInMemoryDatabase("InMem"));

builder.Services.AddScoped<IPlatformRepo,PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Console.WriteLine($"--> CommandService Endpoint {Configuration["CommandService"]}");

var app = builder.Build();

// builder.Services.AddDbContext<AppDbContext>(opt => 
//     opt.UseSqlServer(app.Configuration.GetConnectionString("PlatformsConn")));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app);

app.Run();

