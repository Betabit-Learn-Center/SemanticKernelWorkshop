using Azure;
using Azure.AI.TextAnalytics;
using Azure_Semantic_Kernel_Workshop;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Graph;
using Microsoft.Kiota.Abstractions.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: true);

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
  .AddMicrosoftIdentityWebApp(options =>
  {
      builder.Configuration.GetSection("AzureAd").Bind(options);
      options.TokenValidationParameters.ValidateIssuer = false;
      options.Events = new OpenIdConnectEvents
      {
          OnAccessDenied = context =>
          {
              context.Response.Redirect("/access-denied");
              context.HandleResponse();
              return Task.CompletedTask;
          }
      };
  })
  .EnableTokenAcquisitionToCallDownstreamApi()
  .AddInMemoryTokenCaches();

// Add HttpClient for Graph API calls
builder.Services.AddHttpClient();

builder.Services.AddScoped<GraphServiceClient>(serviceProvider =>
{
    var tokenAcquisition = serviceProvider.GetRequiredService<ITokenAcquisition>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var tokenProvider = new TokenProvider(tokenAcquisition, configuration);
    var authProvider = new BaseBearerTokenAuthenticationProvider(tokenProvider);
    return new GraphServiceClient(authProvider);
});


// Configure logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();

    // Set minimum log level based on environment
    if (builder.Environment.IsDevelopment())
    {
        config.SetMinimumLevel(LogLevel.Debug);
    }
    else
    {
        config.SetMinimumLevel(LogLevel.Information);
    }
});

// Add services to the container
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
    options.Filters.Add<RequireGraphTokenAttribute>();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configuration values
string openAiEndpoint = builder.Configuration["OpenAI:OPENAI_ENDPOINT"]!;
string openAiKey = builder.Configuration["OpenAI:OPENAI_API_KEY"]!;
string openAiDeploymentName = builder.Configuration["OpenAI:OPENAI_DEPLOYMENT_NAME"]!;

var textAnalyticsClient = new TextAnalyticsClient(new Uri(openAiEndpoint), new AzureKeyCredential(openAiKey));

// Register services in DI container 
builder.Services.AddSingleton(textAnalyticsClient);
builder.Services.AddScoped<IGraphService, GraphService>();
builder.Services.AddScoped<INewsService, GoogleRssFeedService>();
builder.Services.AddScoped<IEmailService, OutlookEmailService>();
builder.Services.AddSingleton<ITextAnalysisService, AzureTextAnalytics>();

// Add Semantic Kernel services to the DI container
var kernelBuilder = Kernel.CreateBuilder();
kernelBuilder.Services.AddAzureOpenAIChatCompletion(
    deploymentName: openAiDeploymentName,
    endpoint: openAiEndpoint,
    apiKey: openAiKey);

// Register kernel builder services with the main DI container
foreach (var service in kernelBuilder.Services)
{
    builder.Services.Add(service);
}

// Register kernel as a scoped service that will be built when first requested
builder.Services.AddScoped<Kernel>(serviceProvider =>
{
    var kernel = kernelBuilder.Build();

    // Get services from DI container and create plugins
    var graphService = serviceProvider.GetRequiredService<IGraphService>();
    var newsService = serviceProvider.GetRequiredService<INewsService>();
    var emailService = serviceProvider.GetRequiredService<IEmailService>();
    var textAnalysisService = serviceProvider.GetRequiredService<ITextAnalysisService>();

    // Create plugins with logger injection
    var calendarPlugin = new CalendarPlugin(graphService, serviceProvider.GetRequiredService<ILogger<CalendarPlugin>>());
    var newsPlugin = new NewsPlugin(newsService, serviceProvider.GetRequiredService<ILogger<NewsPlugin>>());
    var emailPlugin = new EmailPlugin(emailService, serviceProvider.GetRequiredService<ILogger<EmailPlugin>>());
    var textAnalysisPlugin = new TextAnalysisPlugin(textAnalysisService, serviceProvider.GetRequiredService<ILogger<TextAnalysisPlugin>>());

    // Register plugins with kernel
    kernel.Plugins.AddFromObject(calendarPlugin);
    kernel.Plugins.AddFromObject(newsPlugin);
    kernel.Plugins.AddFromObject(emailPlugin);
    kernel.Plugins.AddFromObject(textAnalysisPlugin);
    return kernel;
});

builder.Services.AddScoped<IChatCompletionService>(serviceProvider =>
    serviceProvider.GetRequiredService<Kernel>().GetRequiredService<IChatCompletionService>());

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api"))
    {
        context.Request.Headers["X-Api-Key"] = builder.Configuration["API_KEY"];
    }
    await next.Invoke();
});


app.MapControllers();

Console.WriteLine("===============================================");
Console.WriteLine("         🌟 BetaBot Web App Started!  🌟");
Console.WriteLine("===============================================");
Console.WriteLine($"🌐 Open your browser and go to: http://localhost:{app.Urls.FirstOrDefault()?.Split(':').Last() ?? "5000"}");
Console.WriteLine("💡 The web interface is now ready!");
Console.WriteLine("===============================================");

app.Run();

