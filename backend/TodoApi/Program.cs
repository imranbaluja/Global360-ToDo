using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddSingleton<InMemoryTodoService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow CORS for local dev (adjust origins for production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocal");

app.UseRouting();
app.UseAuthorization();

// Root endpoint for home page
app.MapGet("/", () => "Welcome to ToDo api");

app.MapControllers();

app.Run();
