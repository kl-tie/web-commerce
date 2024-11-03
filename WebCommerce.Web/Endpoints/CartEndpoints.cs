using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCommerce.Web.Web.Dtos;
using WebCommerce.Web.Web.Entities;
using System.Runtime.InteropServices;

namespace WebCommerce.Web.Web.Endpoints;

public static class CartEndpoints
{
    public static RouteGroupBuilder MapCartApis(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetCartItems);
        group.MapPost("/", AddCartItem);

        return group;
    }

    public static async Task<Ok<List<CartItemDetail>>> GetCartItems([FromServices] CommerceDbContext db,
        CancellationToken ct)
    {
        var cartItems = await db.CartItems
            .Include(Q => Q.Product)
            .Include(Q => Q.UserAccount)
            .Select(Q => new CartItemDetail
            {
                UserAccountId = Q.UserAccountId,
                FullName = Q.UserAccount.FullName,
                ProductId = Q.ProductId,
                ProductName = Q.Product.Name,
                Price = Q.Product.Price,
                Qty = Q.Qty,
                SubTotal = Q.Product.Price * Q.Qty
            })
            .ToListAsync(ct);

        return TypedResults.Ok(cartItems);
    }

    public static async Task<IResult> AddCartItem([FromServices] CommerceDbContext db,
        [FromBody] CartItemForm body,
        CancellationToken ct)
    {
        // Do validations before inserting...

        var productQty = await db.Products
            .Where(Q => Q.Id == body.ProductId)
            .Select(Q => Q.Qty)
            .FirstOrDefaultAsync(ct);

        if (productQty < body.Qty)
        {
            return Results.BadRequest("Cannot exceed in-stock quantity.");
        }
        
        db.CartItems.Add(new CartItem
        {
            UserAccountId = body.UserAccountId,
            ProductId = body.ProductId,
            Qty = body.Qty
        });

        await db.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}
