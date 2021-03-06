using System;

namespace U.ProductService.Domain.Common
{
    public interface ITrackable
    {
        DateTime CreatedAt { get;  }
        string CreatedBy { get;  }
        DateTime? LastUpdatedAt { get;  }
        string LastUpdatedBy { get;  }
    }
}