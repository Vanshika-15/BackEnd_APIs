using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
namespace BackEnd.Data;

public class conferences
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string? Name { get; set; }

    
}


