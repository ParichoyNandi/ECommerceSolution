using ECommPortal.Models.ValueObjects;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace ECommPortal.Models
{
    public class LMSSMSSyncDashboardViewModel : ResponseModel
    {

        public String ChoosenCrieteria { get; set; }
        public DateTime StartDate { get; set; } = new();
        public DateTime EndDate { get; set; } = new();
    }
}
