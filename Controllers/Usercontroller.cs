using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
    }

[HttpGet]

public IActionResult Users()
{
var users = new List<Users>();
users.Add(new Users{
    
Id =1,
Name ="upneet"

});
users.Add(new Users{
    
Id =2,
Name ="muskan"

});
users.Add(new Users{
    
Id =3,
Name ="vanshika"

});
return Ok(users);
}

}
