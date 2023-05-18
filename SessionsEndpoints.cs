using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace BackEnd;

public static class SessionsEndpoints
{
    public static void MapSessionsEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Sessions").WithTags(nameof(Sessions));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Sessions.ToListAsync();
        })
        .WithName("GetAllSessionss")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Sessions>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.Sessions.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Sessions model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetSessionsById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Sessions sessions, ApplicationDbContext db) =>
        {
            var affected = await db.Sessions
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, sessions.Id)
                  .SetProperty(m => m.Name, sessions.Name)
                  .SetProperty(m => m.Bio, sessions.Bio)
                  .SetProperty(m => m.WebSite, sessions.WebSite)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateSessions")
        .WithOpenApi();

        group.MapPost("/", async (Sessions sessions, ApplicationDbContext db) =>
        {
            db.Sessions.Add(sessions);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Sessions/{sessions.Id}",sessions);
        })
        .WithName("CreateSessions")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.Sessions
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteSessions")
        .WithOpenApi();
    }
}
