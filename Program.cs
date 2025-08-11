using api.ApplicationDbContext;
using api.Interfaces;
using api.Repository;
using api.Service;
using api.Services;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddTransient<IStockService, StockService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddTransient<IBookRepository, BookRepository>();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlite("Data Source=MyFirstWebApi.db");
    
    // Warning ni suppress qilish
    options.ConfigureWarnings(warnings =>
        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGet("/", () => "Hello World!")
    .WithName("GetHelloWorld");
app.MapControllers();
app.Run();
