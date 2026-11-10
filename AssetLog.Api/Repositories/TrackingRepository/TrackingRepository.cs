using AssetLog.Api.Data;
using AssetLog.Api.Models;
using AssetLog.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AssetLog.Api.Repositories.TrackingRepository;

public class TrackingRepository(AppDbContext db) : ITrackingRepository
{
    public async Task<List<Tracking>> GetApproverTrackingsAsync(int approverId, CancellationToken cancellationToken = default)
    {
        return await db.Trackings
            .AsNoTracking()
            .Where(t => t.ApproverId == approverId)
            .Include(t => t.Order)
            .Include(t => t.Approver)
            .Include(t => t.TrackingNotification)
                .ThenInclude(n => n.User)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Tracking>> GetTrackingsByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default)
    {
        return await db.Trackings
            .AsNoTracking()
            .Where(t => t.Status == status)
            .Include(t => t.Order)
            .Include(t => t.Approver)
            .Include(t => t.TrackingNotification)
                .ThenInclude(n => n.User)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Tracking>> GetTrackingsByOrderIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return await db.Trackings
            .AsNoTracking()
            .Where(t => t.OrderId == orderId)
            .Include(t => t.Order)
            .Include(t => t.Approver)
            .Include(t => t.TrackingNotification)
                .ThenInclude(n => n.User)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<Tracking?> GetTrackingByIdAsync(int trackingId, CancellationToken cancellationToken = default)
    {
        return await db.Trackings
            .AsNoTracking()
            .Where(t => t.Id == trackingId)
            .Include(t => t.Order)
            .Include(t => t.Approver)
            .Include(t => t.TrackingNotification)
                .ThenInclude(n => n.User)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Tracking>> GetAllTrackingsAsync(CancellationToken cancellationToken = default)
    {
        return await db.Trackings
            .AsNoTracking()
            .Include(t => t.Order)
            .Include(t => t.Approver)
            .Include(t => t.TrackingNotification)
                .ThenInclude(n => n.User)
            .OrderByDescending(t => t.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task AddTrackingAsync(Tracking tracking, CancellationToken cancellationToken = default)
    {
        await db.Trackings.AddAsync(tracking, cancellationToken);
    }
}