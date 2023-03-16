using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models.ValueObjects
{
    public class ChoosenPlanForActiveDeactive
    {
        public Plan UpdatedPlanLists { get; set; } = new();
        public bool statusvalue { get; set; }
    }
}
