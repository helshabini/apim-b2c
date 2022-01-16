using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("AzureAd", options =>
    {
        options.Authority = "https://login.microsoftonline.com/<tenant-id>";
        options.Audience = "<api-client-id>";
        options.TokenValidationParameters.ValidIssuers = new[]
            {"https://login.microsoftonline.com/<tenant-id>/v2.0"};
        options.TokenValidationParameters.ValidAudiences = new[] {"<api-client-id>"};
    })
    .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAdB2C", options);
        },
        options => { builder.Configuration.Bind("AzureAdB2C", options); }
    );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
