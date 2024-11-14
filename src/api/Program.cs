using System.Text;
using api.dto;
using api.mapper;
using api.model;
using api.service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

#region mapper    
builder.Services.AddAutoMapper(typeof(RecipeProfile));
#endregion

#region database
builder.Services.AddDbContext<RecipeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Recipedb")));
#endregion

#region DI
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddSingleton<AppSettings>(o => new()
{
    AzureEndpoint = configuration["AzureSearch:Endpoint"] ?? string.Empty,
    AzureIndex = configuration["AzureSearch:Index"] ?? string.Empty,
    AzureApiKey = configuration["AzureSearch:Key"] ?? string.Empty,
});


#endregion

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
                {
                    options
                        .AddDefaultPolicy(builder =>
                        {
                            builder
                                .AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
                });

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.UseHttpsRedirection();
}


app.MapPost("/Login", (LoginRequestDto request, IUserService userService) =>
{
    var token = userService.ValidateUser(request);
    return token != null
        ? Results.Ok(new { token })
        : Results.Unauthorized();
});

app.MapGet("/GetUsername", [Authorize] (HttpContext httpContext, ITokenService tokenService) =>
{
    var username = tokenService.GetUsername(httpContext);
    return username != null
        ? Results.Ok(new { Username = username })
        : Results.Unauthorized();
})
.RequireAuthorization();

app.MapPost("/AddRecipes", (List<RecipeDto> recipes, IRecipeService service) =>
{
    service.AddRecipes(recipes);
    return Results.Created();
});

app.MapPost("/Search", [Authorize] async ([FromBody] SearchRequestDto query, ISearchService service, CancellationToken cancellationToken) =>
{
    var result = await service.SearchRecipesAsync(query, cancellationToken);
    return Results.Ok(result);
})
.RequireAuthorization();

app.Run();

