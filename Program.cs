using StudyBuddy.API.DbConnecitionFactory;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Repository;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Services;
using StudyBuddy.API.Services.Interface;
using StudyBuddy.API.Services.Mappings;
using StudyBuddy.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Database connection factory
builder.Services.AddScoped<IDbConnectionFactory, MsSqlDbConnectionFactory>();

// Generic repository
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

// Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IOptionRepository, OptionRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMatchRequestRepository, MatchRequestRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IOptionService, OptionService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IMatchRequestService, MatchRequestService>();
builder.Services.AddScoped<IUserService, UserService>();

// Matching algorithm service
builder.Services.AddScoped<IMatchingService, MatchingService>();

var app = builder.Build();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();