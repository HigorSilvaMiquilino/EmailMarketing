using EmailMarketing.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();



app.UseStaticFiles();
app.UseDefaultFiles();



app.UseRouting();
app.MapGet("/", async (HttpContext context) =>
{
    context.Response.Redirect("/html/index.html");
});



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
