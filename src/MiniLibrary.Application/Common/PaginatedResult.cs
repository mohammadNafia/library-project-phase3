using System.Collections.Generic;
using System.Linq;

namespace MiniLibrary.Application.Common;

public class PaginatedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
