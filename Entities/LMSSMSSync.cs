using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class LMSSMSSync
    {
       public string StudentCode { get; set; }
       public string StudentName { get; set; }
       public string Address { get; set; }
       public int courseID { get; set; }
       public String GuardianName { get; set; }
       public long ContactNo { get; set; }
       public DateTime DateOfBirth { get; set; }
       public DateTime DateOfJoining { get; set; }
       public string createdBy { get; set; } = "rice-group-admin";
    }
}

