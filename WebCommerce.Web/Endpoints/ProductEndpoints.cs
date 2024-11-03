using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCommerce.Web.Web.Dtos;
using WebCommerce.Web.Web.Entities;

namespace WebCommerce.Web.Web.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductApis(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetProducts);
        group.MapPost("/", CreateProduct);

        return group;
    }

    public static async Task<Ok<List<ProductDetail>>> GetProducts([FromServices] CommerceDbContext db)
    {
        // Instead of doing these...
//        var dbConn = db.Database.GetDbConnection();

//        var query = @"
//SELECT p.id,
//    p.name,
//    p.price,
//    p.qty
//FROM products p
//WHERE p.deleted_at IS NULL
//ORDER BY id DESC
//LIMIT 10
//OFFSET 0";

//        var products = (await dbConn.QueryAsync<ProductDetail>(query))
//            .ToList();

        // We are doing these.
        var products = await db.Products
            .Where(Q => Q.DeletedAt == null)
            .Select(Q => new ProductDetail
            {
                Id = Q.Id,
                Name = Q.Name,
                Price = Q.Price,
                Qty = Q.Qty
            })
            .OrderByDescending(Q => Q.Id)
            .Skip(0) // OFFSET
            .Take(10)// LIMIT
            .ToListAsync();

        return TypedResults.Ok(products);
    }

    public static async Task<Ok<Guid>> CreateProduct([FromServices] CommerceDbContext db,
        [FromBody] ProductForm body,
        CancellationToken ct)
    {
        // Do validations before inserting...
        var newProductId = Guid.NewGuid();

        db.Products.Add(new Product
        {
            Id = newProductId,
            Name = body.Name,
            Price = body.Price,
            Qty = body.Qty
        });

        await db.SaveChangesAsync(ct);

        return TypedResults.Ok(newProductId);
    }
}
