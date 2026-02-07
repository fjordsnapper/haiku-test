using HaikuApi.Configuration;
using HaikuApi.Services;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add Application Insights
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddSingleton<ITelemetryService, TelemetryService>();

// Add feature flags configuration
builder.Services.Configure<FeatureFlags>(builder.Configuration.GetSection("FeatureFlags"));
builder.Services.AddSingleton<IFeatureFlagService, FeatureFlagService>();

// Add Azure AD authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserService>(provider =>
    new UserServiceWithTelemetry(
        provider.GetRequiredService<UserService>(),
        provider.GetRequiredService<IFeatureFlagService>(),
        provider.GetRequiredService<ITelemetryService>()
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
