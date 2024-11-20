using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

// var builder = WebApplication.CreateBuilder(args);

// var jwtOptions = builder.Configuration
// 	.GetSection("JwtOptions")
//     .Get<JwtOptions>();

// builder.Services.AddSingleton(jwtOptions);

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         byte[] signingKeyBytes = Encoding.UTF8
//         	.GetBytes(jwtOptions.SigningKey);

//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ClockSkew = TimeSpan.Zero,
//             ValidIssuer =  jwtOptions.Issuer,
//             ValidAudience = jwtOptions.Audience,
//             IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
//         };
//     });

// builder.Services.AddAuthorization();

// // Add services to the container
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", builder =>
//         builder.AllowAnyOrigin()
//                .AllowAnyMethod()
//                .AllowAnyHeader());
// });

// builder.Services.AddControllers()
//     .AddJsonOptions(options =>
//     {
//         options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
//     });

// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<TaskManagerContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(c =>
//     {
//         c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
//         c.RoutePrefix = string.Empty; 
//     });
// }

// app.UseHttpsRedirection();

// app.UseCors("AllowAll");

// app.UseAuthentication();
// app.UseAuthorization();

// app.MapControllers();
// app.MapGet("/", () => "Hello World!");
// app.MapGet("/public", () => "Public Hello World!")
// 	.AllowAnonymous();

// app.MapGet("/private", () => "Private Hello World!")
// 	.RequireAuthorization();

// app.MapPost("/tokens/connect", (HttpContext ctx, JwtOptions jwtOptions)
//     => TokenEndpoint.Connect(ctx, jwtOptions));

// app.Run();


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<ISenhaInterface, SenhaService>();

builder.Services.AddDbContext<TaskManagerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Use o cabeçalho Authorization com o esquema Bearer: \"Bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var tokenKey = builder.Configuration.GetSection("AppSettings:Token").Value;

if (string.IsNullOrEmpty(tokenKey))
{
    throw new ArgumentNullException("AppSettings:Token", "A chave secreta do JWT (AppSettings:Token) não está configurada no appsettings.json ou nas variáveis de ambiente.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API V1");
        c.DocumentTitle = "Documentação da API TaskManager";
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();