using Blazor.Components;
using Blazor.Components.Services;
using BlazorApp.HttpServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("ForumApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5036/");
});
builder.Services.AddScoped<IUserService>(sp =>
    new HttpUserService(
        sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("ForumApi")
    ));
builder.Services.AddScoped<IPostService>(sp =>
    new HttpPostService(
        sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("ForumApi")
    ));


builder.Services.AddScoped<ICommentService>(sp =>
    new HttpCommentService(
        sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("ForumApi")
    ));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();