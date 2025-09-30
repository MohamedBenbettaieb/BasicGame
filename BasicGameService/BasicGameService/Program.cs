var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Development diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // shows full stack trace locally
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// serve static files (important)
app.UseStaticFiles();

// routing & auth
app.UseRouting();
app.UseAuthorization();

// If MapStaticAssets is an app-specific extension, keep it but ensure its namespace is imported
// If you don't know where it comes from, comment these two lines out to test.
try
{
    app.MapStaticAssets();
}
catch
{
    // swallow here only to help local debugging; remove this try/catch in production code
}

// Map controllers/routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
   .WithStaticAssets(); // again, ensure this extension exists; comment out to test if unsure

app.Run();
