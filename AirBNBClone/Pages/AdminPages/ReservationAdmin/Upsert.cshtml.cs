using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.AdminPages.ReservationAdmin
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        [BindProperty]
        public Reservation objReservation { get; set; }

        public UpsertModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objReservation = new Reservation();
        }

        public IActionResult OnGet(int? id)
        {
            objReservation = new Reservation();

            if (id != 0 && id.HasValue)
            {
                objReservation = _unitOfWork.Reservation.GetById(id);
            }

            if (objReservation == null)
            {
                return NotFound();
            }

            objReservation.Rental = _unitOfWork.Rental.Get(x => x.Id == objReservation.RentalId);
            objReservation.User = _unitOfWork.ApplicationUser.Get(x => x.Id == objReservation.UserId);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (objReservation.Id == 0)
            {
                _unitOfWork.Reservation.Add(objReservation);
                TempData["success"] = "Reservation added successfully";
            }
            else
            {
                _unitOfWork.Reservation.Update(objReservation);
                TempData["success"] = "Reservation updated successfully";
            }
            _unitOfWork.Commit();

            return RedirectToPage("./Index");
        }
    }
}
