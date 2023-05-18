using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace BackEnd;

public static class ConferenceAttendeeEndpoints
{
    public static void MapConferenceAttendeeEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ConferenceAttendee").WithTags(nameof(ConferenceAttendee));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.ConferenceAttendee.ToListAsync();
        })
        .WithName("GetAllConferenceAttendees")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<ConferenceAttendee>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.ConferenceAttendee.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is ConferenceAttendee model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetConferenceAttendeeById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, ConferenceAttendee conferenceAttendee, ApplicationDbContext db) =>
        {
            var affected = await db.ConferenceAttendee
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, conferenceAttendee.Id)
                  .SetProperty(m => m.Name, conferenceAttendee.Name)
                  .SetProperty(m => m.Bio, conferenceAttendee.Bio)
                  .SetProperty(m => m.WebSite, conferenceAttendee.WebSite)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateConferenceAttendee")
        .WithOpenApi();

        group.MapPost("/", async (ConferenceAttendee conferenceAttendee, ApplicationDbContext db) =>
        {
            db.ConferenceAttendee.Add(conferenceAttendee);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/ConferenceAttendee/{conferenceAttendee.Id}",conferenceAttendee);
        })
        .WithName("CreateConferenceAttendee")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.ConferenceAttendee
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteConferenceAttendee")
        .WithOpenApi();
    }
}
