using Microsoft.AspNetCore.Identity;
using ExpiredFood_BackEnd.Data;
using ExpiredFood.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()  // Permitir cualquier origen
                  .AllowAnyHeader()  // Permitir cualquier encabezado
                  .AllowAnyMethod(); // Permitir cualquier método HTTP (GET, POST, PUT, DELETE, etc.)
        });
});

// Agregar servicios de Swagger
builder.Services.AddEndpointsApiExplorer();

//Adding the security definition in Swagger:
builder.Services.AddSwaggerGen(options =>{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//Connection String
var connectionString = builder.Configuration.GetConnectionString("ExpiredFoodContext");
builder.Services.AddSqlServer<ExpiredFood_BackEndContext>(connectionString);

var connectionString2 = builder.Configuration.GetConnectionString("AuthenticationContext");
builder.Services.AddSqlServer<AuthenticationContext>(connectionString2);

builder.Services.AddDbContext<AuthenticationContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString2")));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<AuthenticationContext>();

var app = builder.Build();

// Habilitar CORS
app.UseCors("AllowAllOrigins");

app.Services.InitializeDb();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Weeeena po!");
app.MapIdentityApi<IdentityUser>();
app.MapCategoriesEndpoints();
app.MapTransactionsEndpoints();
app.MapFoodsEndpoints();
app.MapUsersEndpoints();

app.Run();
