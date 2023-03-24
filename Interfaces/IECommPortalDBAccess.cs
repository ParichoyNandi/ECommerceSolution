using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Interfaces
{
    public interface IEcommPortalDBAccess
    {
        Plan GetPlanDetails(int planId);
        int UpdatePlanDetails(Plan plan);
        int UpdatePlanConfig(Plan plan);
    }
}
