using System;
using Microsoft.AspNetCore.Mvc;
using AssetLog.Api.DTOs;
using AssetLog.Api.Services.UserServices;

namespace AssetLog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    // GET /user ->  user
    [HttpGet()]
    public async Task<ActionResult<List<UserDto>>> GetUsers(
        CancellationToken cancellationToken)
    {
        var users = await userService.GetAllUsers();
        return Ok(users);
    }

    // GET /user/{userid} ->  user id
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserDto>> GetUserId(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetUsersById(userId);
        return Ok(user);
    }

}


