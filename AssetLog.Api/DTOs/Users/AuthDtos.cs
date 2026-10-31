using System.ComponentModel.DataAnnotations;
using AssetLog.Api.Models.Enums;

namespace AssetLog.Api.DTOs;

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password);

public record LoginResponse(string Token, DateTime ExpiresAt, UserDto User);