using AssetLog.Api.Models;
using AssetLog.Api.Models.Enums;

namespace AssetLog.Api.Repositories.TrackingRepository
{
    public interface ITrackingRepository
    {
        Task<List<Tracking>> GetApproverTrackingsAsync(int approverId, CancellationToken cancellationToken = default);
        Task<List<Tracking>> GetTrackingsByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default);
        Task<List<Tracking>> GetTrackingsByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
        Task<Tracking?> GetTrackingByIdAsync(int trackingId, CancellationToken cancellationToken = default);
        Task<List<Tracking>> GetAllTrackingsAsync(CancellationToken cancellationToken = default);
        Task AddTrackingAsync(Tracking tracking, CancellationToken cancellationToken = default);

    }
}