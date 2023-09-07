using SURAKSHA;
using SURAKSHA.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using SURAKSHA.Models.QueryModel;
using Azure.Storage.Blobs;
using RMS_API.Models.QueryModel;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().
    ReadFrom.Configuration(new ConfigurationBuilder()
   .AddJsonFile("seri-log.config.json").Build())
    .Enrich.FromLogContext().
    CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);



// Add services to the container.

builder.Services.AddControllers();
  //.AddJsonOptions(
  //              x =>
  //              {
  //                  x.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
  //              });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(_ =>
{
    return new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage"));
});

builder.Services.AddScoped<IFileService, FileService>();



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();
AppSettingsHelper.AppSettingsConfigure(configuration : app.Services.GetRequiredService<IConfiguration>());



// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
    app.UseSwagger();
    app.UseSwaggerUI();
//app.UseMiddleware<HttpLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

