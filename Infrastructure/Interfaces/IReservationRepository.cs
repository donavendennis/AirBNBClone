using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IReservationRepository<T> : IGenericRepository<Reservation>
    {
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);

        void UpdateStripePaymentID(int id, string sessionId, string InvoiceId);
    }
}
