using MiniLibrary.Application.Services;
using Domain.Interfaces;

namespace MiniLibrary.Infrastructure.Services;

public class AuditService : IAuditService
{
    public void TrackChanges(IAuditable entity, Dictionary<string, object> changes)
    {
       
        entity.Changes = string.Join(", ",
            changes.Select(c => $"{c.Key}: {c.Value}"));

        entity.UpdatedAt = DateTime.UtcNow;
    }
}