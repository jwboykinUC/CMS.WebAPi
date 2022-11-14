using CMS.WebAPi.CMS.Data.Repository.Repositories;
using CMS.WebAPi.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICmsRepository, InMemoryCmsRepository>();
builder.Services.AddAutoMapper(typeof(CmsMapper));
//builder.Services.AddApiVersioning();  //add default versioning
builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);

    //setupAction.ApiVersionReader = new QueryStringApiVersionReader("v");  //query string versioning
    //setupAction.ApiVersionReader = new UrlSegmentApiVersionReader(); //url versioning
    // setupAction.ApiVersionReader = new HeaderApiVersionReader("X-Version"); //header versioning
    setupAction.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("v"),
     new HeaderApiVersionReader("X-Version")
     );  //combined versioning
});  //add versioning
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

app.UseAuthorization();

app.MapControllers();

app.Run();
