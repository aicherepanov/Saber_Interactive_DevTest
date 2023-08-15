using ListSerializer.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ListSerializer",
        Description = "Solution of the test task for the vacancy of QA Automation Engineer in Saber Interactive. Implementation of the IListSerializer interface.",
        Contact = new OpenApiContact
        {
            Name = "ListSerializer WebApi",
            Email = string.Empty,
            Url = new Uri("https://github.com/aicherepanov")
        }
    });
});

builder.Services.RegisterProjectDependencies();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();