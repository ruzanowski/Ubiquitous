using System;

namespace U.ProductService.Domain.Aggregates
{
    public interface ITrackable
    {
        DateTime CreatedAt { get;  }
        string CreatedBy { get;  }
        DateTime? LastUpdatedAt { get;  }
        string LastUpdatedBy { get;  }
    }
}