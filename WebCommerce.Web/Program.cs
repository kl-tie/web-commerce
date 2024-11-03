using WebCommerce.Web.Web.Endpoints;
using Microsoft.EntityFrameworkCore;
using WebCommerce.Web.Web.Entities;
using WebCommerce.Web.Web.Interceptors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("Sql");

builder.Services.AddScoped<AuditTrailInterceptor>();

builder.Services.AddDbContext<CommerceDbContext>((sp, o) =>
{
    var auditTrailInterceptor = sp.GetRequiredService<AuditTrailInterceptor>();
    o.UseNpgsql(connString);
    o.UseSnakeCaseNamingConvention();
    o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    o.AddInterceptors(auditTrailInterceptor);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("/v1/users")
    .MapUserApis();
app.MapGroup("/v1/products")
    .MapProductApis();
app.MapGroup("/v1/carts")
    .MapCartApis();
app.MapGroup("/v1/checkout")
    .MapCheckoutApis();

app.Run();
