using AssetLog.Api.DTOs.Tracking;
using AssetLog.Api.Models.Enums;

namespace AssetLog.Api.Services.TrackingService
{
    public interface ITrackingService
    {
        Task<List<TrackingDto>> GetApproverTrackingsAsync(int approverId, CancellationToken cancellationToken = default);
        Task<List<TrackingDto>> GetTrackingsByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default);
        Task<List<TrackingDto>> GetTrackingsByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
        Task<TrackingDto?> GetTrackingByIdAsync(int trackingId, CancellationToken cancellationToken = default);
        Task<List<TrackingDto>> GetAllTrackingsAsync(CancellationToken cancellationToken = default);
        
    }
}