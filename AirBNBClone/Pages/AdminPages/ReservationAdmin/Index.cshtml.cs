using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Models;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace AirBNBClone.Pages.AdminPages.ReservationAdmin
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Reservation> Reservations { get; set; }

        public async Task OnGetAsync()
        {
            Reservations = (await _unitOfWork.Reservation.GetAllAsync()).ToList();
            foreach (var reservation in Reservations)
            {
                reservation.User = await _unitOfWork.ApplicationUser.GetAsync(x => x.Id == reservation.UserId);
            }
        }
    }
}