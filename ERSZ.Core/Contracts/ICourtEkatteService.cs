using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSZ.Core.Contracts
{
    public interface ICourtEkatteService : IBaseService
    {
       IQueryable GetAllByDistrictMapId(string districtMapId);
    }
}
