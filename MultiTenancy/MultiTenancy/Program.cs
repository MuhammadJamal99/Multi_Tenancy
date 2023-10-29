var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

TenantSettings options = new();
builder.Configuration.GetSection(nameof(TenantSettings)).Bind(options);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
