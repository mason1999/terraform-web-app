using Azure.Data.Tables;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews(); // Use this to enable both controllers and views

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure TableClient and register the TodoService
builder.Services.AddSingleton(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("TableStorage") ?? Environment.GetEnvironmentVariable("TableStorage") ?? builder.Configuration["TableStorage"];

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("TableStorage connection string is missing.");
    }   

    var tableClient = new TableClient(connectionString, "TodoItems");
    tableClient.CreateIfNotExists(); // Ensure the table exists
    return tableClient;
});
builder.Services.AddScoped<TodoService>();

var app = builder.Build();

// Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use routing for the controllers and views
app.UseRouting();

// Define default controller route (HomeController, Index action)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// app.UseHttpsRedirection();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
