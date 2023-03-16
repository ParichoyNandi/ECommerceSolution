using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPaymentManagement
    {
        void ProcessPayment(Payment pay);
        int ProcessSubscriptionPayment(StudentSubscriptionTransaction transaction, int FeeScheduleID);
        void ProcessExistingTransaction(Transaction pay, string TransactionStatus);
        int ProcessPayoutPayment(StudentPayOutTransaction transaction, int FeeScheduleID);
        int ProcessExistingPayoutPayment(StudentPayOutTransaction transaction, string TransactionStatus);
    }
}
