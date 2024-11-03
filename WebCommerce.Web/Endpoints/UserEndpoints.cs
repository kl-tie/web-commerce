using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCommerce.Web.Web.Dtos;
using WebCommerce.Web.Web.Entities;

namespace WebCommerce.Web.Web.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserApis(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetUsers);
        group.MapPost("/", AddUser);

        return group;
    }

    public static async Task<Ok<List<UserDetail>>> GetUsers([FromServices] CommerceDbContext db,
        CancellationToken ct)
    {
        var users = await db.UserAccounts
            .Select(Q => new UserDetail
            {
                UserAccountId = Q.Id,
                FullName = Q.FullName
            })
            .ToListAsync(ct);

        return TypedResults.Ok(users);
    }

    public static async Task<Ok<Guid>> AddUser([FromServices] CommerceDbContext db,
        [FromBody] UserForm body,
        CancellationToken ct)
    {
        // Do validations before inserting...
        var newUserId = Guid.NewGuid();

        db.UserAccounts.Add(new UserAccount
        {
            Id = newUserId,
            FullName = body.FullName
        });

        await db.SaveChangesAsync(ct);

        return TypedResults.Ok(newUserId);
    }
}
