using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Infrastructure.Contracts
{
    public interface IOrderable
    {
        int Id { get; set; }
        int OrderNumber { get; set; }
    }
}
