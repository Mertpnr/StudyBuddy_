using StudyBuddy.API.DbConnecitionFactory;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Repository;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Services.Interface;
using StudyBuddy.API.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMatchingService, MatchingService>();
builder.Services.AddScoped<IDbConnectionFactory, MsSqlDbConnectionFactory>();
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

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
