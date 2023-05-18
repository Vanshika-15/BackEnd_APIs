using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
namespace BackEnd.Data;

public class ConferenceAttendee
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string? Name { get; set; }

    [StringLength(4000)]
    public string? Bio { get; set; }

    [StringLength(1000)]
    public virtual string? WebSite { get; set; }
}