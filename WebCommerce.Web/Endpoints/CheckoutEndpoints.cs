using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCommerce.Web.Web.Dtos;
using WebCommerce.Web.Web.Entities;

namespace WebCommerce.Web.Web.Endpoints;

public static class CheckoutEndpoints
{
    public static RouteGroupBuilder MapCheckoutApis(this RouteGroupBuilder group)
    {
        group.MapPost("/", CheckoutCartItems);

        return group;
    }

    public static async Task<IResult> CheckoutCartItems([FromServices] CommerceDbContext db,
        [FromBody] CheckoutForm form,
        CancellationToken ct)
    {
        var receiptId = Guid.NewGuid();
        var receipt = new Receipt
        {
            Id = receiptId,
            UserAccountId = form.UserAccountId
        };

        db.Receipts.Add(receipt);

        var cartItems = await db.CartItems
            .Include(Q => Q.Product)
            .AsTracking()
            .Where(Q => Q.UserAccountId == form.UserAccountId)
            .ToListAsync(ct);

        foreach (var cartItem in cartItems)
        {
            db.ReceiptDetails.Add(new ReceiptDetail
            {
                ReceiptId = receipt.Id,
                Id = Guid.NewGuid(),
                ProductId = cartItem.ProductId,
                Price = cartItem.Product.Price,
                Qty = cartItem.Qty
            });

            cartItem.Product.Qty -= cartItem.Qty;
        }

        db.RemoveRange(cartItems);

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException e)
        {
            // Log the exception using ILogger...
            return Results.BadRequest($"{e.Entries.FirstOrDefault()} quantity has been updated, please update your cart qty & redo the transaction.");
        }

        return TypedResults.Ok();
    }
}
