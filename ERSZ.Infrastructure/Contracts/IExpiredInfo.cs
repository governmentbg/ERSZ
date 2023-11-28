using System;
using System.Collections.Generic;
using System.Text;

namespace ERSZ.Infrastructure.Contracts
{
    public interface IExpiredInfo
    {
        DateTime? DateExpired { get; set; }

        string ExpiredUserId { get; set; }

        string ExpiredDescription { get; set; }
    }
}
