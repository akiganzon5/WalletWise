using FinanceAPI.Extensions;
using FinanceAPI.Services.FinanceAPI.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCors", policy =>
    {
        policy.WithOrigins("https://bikershub.org")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


builder.Services.AddDbContextServices(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAppServices();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("MyCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
