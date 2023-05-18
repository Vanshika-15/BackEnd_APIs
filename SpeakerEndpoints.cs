using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace BackEnd;

public static class SpeakerEndpoints
{
    public static void MapSpeakerEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Speaker").WithTags(nameof(Speaker));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Speakers.ToListAsync();
        })
        .WithName("GetAllSpeakers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Speaker>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.Speakers.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Speaker model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetSpeakerById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Speaker speaker, ApplicationDbContext db) =>
        {
            var affected = await db.Speakers
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, speaker.Id)
                  .SetProperty(m => m.Name, speaker.Name)
                  .SetProperty(m => m.Bio, speaker.Bio)
                  .SetProperty(m => m.WebSite, speaker.WebSite)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateSpeaker")
        .WithOpenApi();

        group.MapPost("/", async (Speaker speaker, ApplicationDbContext db) =>
        {
            db.Speakers.Add(speaker);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Speaker/{speaker.Id}",speaker);
        })
        .WithName("CreateSpeaker")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.Speakers
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteSpeaker")
        .WithOpenApi();
    }
	public static void MapUsersEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Users").WithTags(nameof(Users));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Users.ToListAsync();
        })
        .WithName("GetAllUserss")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Users>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.Users.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Users model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUsersById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Users users, ApplicationDbContext db) =>
        {
            var affected = await db.Users
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, users.Id)
                  .SetProperty(m => m.Name, users.Name)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUsers")
        .WithOpenApi();

        group.MapPost("/", async (Users users, ApplicationDbContext db) =>
        {
            db.Users.Add(users);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Users/{users.Id}",users);
        })
        .WithName("CreateUsers")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.Users
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUsers")
        .WithOpenApi();
    }
}
