using BackEndAPIFondosBTG.Models;
using BackEndAPIFondosBTG.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddSingleton<MongoDBService>();

builder.Services.AddSingleton<EmailService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new EmailService(
        configuration["EmailSettings:FromEmailAddress"],
        configuration["EmailSettings:SmtpServer"],
        int.Parse(configuration["EmailSettings:SmtpPort"]),
        configuration["EmailSettings:SmtpUsername"],
        configuration["EmailSettings:SmtpPassword"]
    );
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
