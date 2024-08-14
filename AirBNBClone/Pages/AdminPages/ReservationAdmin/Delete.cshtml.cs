using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.AdminPages.ReservationAdmin
{
    public class DeleteModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        [BindProperty]
        public Reservation objReservation { get; set; }

        public DeleteModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objReservation = new Reservation();
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            objReservation = _unitOfWork.Reservation.GetById(id);

            if (objReservation == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var reservationFromDb = _unitOfWork.Reservation.GetById(objReservation.Id);

            if (reservationFromDb == null)
            {
                return NotFound();
            }

            _unitOfWork.Reservation.Delete(reservationFromDb);
            _unitOfWork.Commit();

            TempData["success"] = "Reservation deleted successfully";
            return RedirectToPage("./Index");
        }
    }
}
