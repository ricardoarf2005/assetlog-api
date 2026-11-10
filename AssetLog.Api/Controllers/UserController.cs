using AssetLog.Api.DTOs;
using AssetLog.Api.Models;
using AssetLog.Api.Services.EquipmentService;
using AssetLog.Api.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace AssetLog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "MANAGER")]
public class UsersController(IUserService userService, IEquipmentService equipmentService) : ControllerBase
{
    // GET /users ->  users
    [HttpGet()]
    public async Task<ActionResult<List<UserDto>>> GetUsers(
        CancellationToken cancellationToken)
    {
        var users = await userService.GetAllUsers();
        return Ok(users);
    }

    // GET /users/me ->  my user info
    [HttpGet("me")]
    [Authorize()]
    [AllowAnonymous]
    public async Task<ActionResult<List<UserDto>>> GetMyUser(
        CancellationToken cancellationToken)
    {
        var userNameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userNameIdentifier, out int id) == false)
            return NotFound();

        var users = await userService.GetUsersById(id);
        return Ok(users);
    }

    // GET /users/{userid} ->  user id
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserDto>> GetUserId(
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetUsersById(userId);
        return Ok(user);
    }

    // GET /users/me/material-em-posse 
    [HttpGet("me/material-em-posse")]
    [Authorize()]
    [AllowAnonymous]
    public async Task<ActionResult<EquipmentDto>> GetEquipmentByUserId(
        CancellationToken cancellationToken)
    {
        var userNameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userNameIdentifier, out int id) == false)
            return NotFound();

        
        var equipment = await equipmentService.GetEquipmentByUserId(id);
        return Ok(equipment);
    }

    // POST /users ->  create user
    [HttpPost]
    [Authorize()]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(CreateUserDto newUser)
    {
        var user = await userService.CreateUser(newUser);
        return CreatedAtAction(nameof(CreateUser), new { userId = user.Id }, user);
    }

    // PUT /users/{userId} ->  update user
    [HttpPut("{userId:int}")]
    public async Task<IActionResult> UpdateUser(int userId, UserDto updatedUser)
    {
        var user = await userService.UpdateUser(userId, updatedUser);
        return Ok(user);
    }

    // DELETE /users/{userId} ->  delete user
    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await userService.DeleteUser(userId);
        return Ok(new { message = "User deleted successfully." });
    }
}


