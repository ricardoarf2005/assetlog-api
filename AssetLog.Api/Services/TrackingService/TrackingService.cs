using AssetLog.Api.DTOs.Tracking;
using AssetLog.Api.Models;
using AssetLog.Api.Models.Enums;
using AssetLog.Api.Repositories.TrackingRepository;

namespace AssetLog.Api.Services.TrackingService
{
    public class TrackingService(ITrackingRepository trackingRepository) : ITrackingService
    {
        public async Task<List<TrackingDto>> GetApproverTrackingsAsync(int approverId, CancellationToken cancellationToken = default)
        {
            var trackings = await trackingRepository.GetApproverTrackingsAsync(approverId, cancellationToken);
            return trackings.Select(MapToTrackingDto).ToList();
        }

        public async Task<List<TrackingDto>> GetTrackingsByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default)
        {
            var trackings = await trackingRepository.GetTrackingsByStatusAsync(status, cancellationToken);
            return trackings.Select(MapToTrackingDto).ToList();
        }

        public async Task<List<TrackingDto>> GetTrackingsByOrderIdAsync(int orderId, CancellationToken cancellationToken = default)
        {
            var trackings = await trackingRepository.GetTrackingsByOrderIdAsync(orderId, cancellationToken);
            return trackings.Select(MapToTrackingDto).ToList();
        }

        public async Task<TrackingDto?> GetTrackingByIdAsync(int trackingId, CancellationToken cancellationToken = default)
        {
            var tracking = await trackingRepository.GetTrackingByIdAsync(trackingId, cancellationToken);
            return tracking is null ? null : MapToTrackingDto(tracking);
        }

        public async Task<List<TrackingDto>> GetAllTrackingsAsync(CancellationToken cancellationToken = default)
        {
            var trackings = await trackingRepository.GetAllTrackingsAsync(cancellationToken);
            return trackings.Select(MapToTrackingDto).ToList();
        }

        private static TrackingDto MapToTrackingDto(Tracking tracking)
        {
            return new TrackingDto
            {
                Id = tracking.Id,
                OrderId = tracking.OrderId,
                Status = tracking.Status,
                ApproverId = tracking.ApproverId,
                ApproverName = tracking.Approver?.Name ?? string.Empty,
                Date = tracking.Date,
                Comments = tracking.Comments,
                Notification = tracking.TrackingNotification is null
                    ? null
                    : new TrackingNotificationDto
                    {
                        Id = tracking.TrackingNotification.Id,
                        UserId = tracking.TrackingNotification.UserId,
                        TrackingId = tracking.TrackingNotification.TrackingId,
                        UserName = tracking.TrackingNotification.User?.Name ?? string.Empty,
                        Message = tracking.TrackingNotification.Message,
                        Date = tracking.TrackingNotification.Date,
                        IsRead = tracking.TrackingNotification.IsRead
                    }
            };
        }
    }
}