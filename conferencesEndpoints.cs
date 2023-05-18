using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace BackEnd;

public static class conferencesEndpoints
{
    public static void MapconferencesEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/conferences").WithTags(nameof(conferences));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.conferences.ToListAsync();
        })
        .WithName("GetAllconferencess")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<conferences>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.conferences.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is conferences model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetconferencesById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, conferences conferences, ApplicationDbContext db) =>
        {
            var affected = await db.conferences
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, conferences.Id)
                  .SetProperty(m => m.Name, conferences.Name)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("Updateconferences")
        .WithOpenApi();

        group.MapPost("/", async (conferences conferences, ApplicationDbContext db) =>
        {
            db.conferences.Add(conferences);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/conferences/{conferences.Id}",conferences);
        })
        .WithName("Createconferences")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.conferences
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("Deleteconferences")
        .WithOpenApi();
    }
}
