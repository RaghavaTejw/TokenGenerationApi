using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TokenGenerationApi;
using TokenGenerationApi.Authorization;
using TokenGenerationApi.Models;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {

            //define which claim requires to check
            ValidateIssuer = true, // Validates the server that generates the token
            ValidateAudience = true, // validates the recipient of the token 
            ValidateLifetime = true, // validates the lifetime of the token
            ValidateIssuerSigningKey = true, // validates the signature of the token

            //store the value in appsettings.json and getting the values from the appsettings.json
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            RequireSignedTokens = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


builder.Services.AddScoped<IAuthorizationHandler,PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TokenClass>();

builder.Services.AddDbContext<UserContext>(options =>
options.UseSqlServer(builder.Configuration["ConnectionString:con"]));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
