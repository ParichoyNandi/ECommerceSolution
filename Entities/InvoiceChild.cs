using System;
using System.Collections.Generic;

namespace Entities
{
    public class InvoiceChild
    {
        public int CourseID { get; set; }

        public int InvoiceChildID { get; set; }

        public int InvoiceHeaderID { get; set; }

        public double CourseDiscount { get; set; }

        public double CourseAmount { get; set; }

        public double CourseTaxAmount { get; set; }

        public string IsLumpSum { get; set; }

        public int CourseFeePlanID { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }

        public List<InvoiceChildDetails> InvoiceChildDetails { get; set; } = new();

        public int BatchID { get; set; }

        public int DiscountSchemeId { get; set; }

        public int DiscountAppliedAt { get; set; }
    }
}