#pragma warning disable CS8602

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

using DAL_Layer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("UserContext"));
});


// Configure HTTP Clients
builder.Services.AddHttpClient("EventService", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("APIS:Event"));
    httpClient.Timeout = TimeSpan.FromMilliseconds(2000);
});

// CORS Configuration
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { 
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "User API",
        Description = "An API used for user authentification",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    try
    {
        //app.UseExceptionHandler("/Home/Error");

        using (IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            serviceScope.ServiceProvider.GetRequiredService<UserContext>().Database.Migrate();
        }
    }
    catch { }
}


app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();
