using Domain.Interfaces;

namespace MiniLibrary.Application.Services;

public interface IAuditService
{
    void TrackChanges(IAuditable entity, Dictionary<string, object> changes);
}