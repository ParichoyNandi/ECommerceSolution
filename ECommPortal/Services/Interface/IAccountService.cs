using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IAccountService
    {

        Task<int> Validateuserapi(string Loginid, string password);
    }
}
