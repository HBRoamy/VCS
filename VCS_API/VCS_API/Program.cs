using VCS_API.Middlewares;
using VCS_API.Repositories;
using VCS_API.Repositories.Interfaces;
using VCS_API.Services;
using VCS_API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddSingleton<IRepoService, RepoService>();
builder.Services.AddSingleton<IBranchService, BranchService>();
builder.Services.AddSingleton<ICommitService, CommitService>();
builder.Services.AddTransient<IComparisonService, ComparisonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<RequestTimingMiddleware>();

app.Run();
