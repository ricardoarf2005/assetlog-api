// AssetLog.Api/Controllers/TrackingController.cs
using AssetLog.Api.DTOs.Tracking;
using AssetLog.Api.Services.TrackingService;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AssetLog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrackingController(ITrackingService trackingService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAllTrackings(CancellationToken cancellationToken)
    {
        var result = await trackingService.GetAllTrackingsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByTrackingId([Range(1, int.MaxValue)] int id, CancellationToken cancellationToken)
    {
        var result = await trackingService.GetTrackingByIdAsync(id, cancellationToken);
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpGet("order/{orderId:int}")]
    public async Task<IActionResult> GetTrackingsByOrderId([Range(1, int.MaxValue)] int orderId, CancellationToken cancellationToken)
    {
        var result = await trackingService.GetTrackingsByOrderIdAsync(orderId, cancellationToken);
        return Ok(result);
    }

}