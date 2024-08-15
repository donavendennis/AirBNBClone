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
        private static DateTime tempOrderDate;

        public UpsertModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objReservation = new Reservation();
        }

        public IActionResult OnGet(int? id)
        {
            System.Diagnostics.Debug.WriteLine("Trigger OnGet");
            objReservation = new Reservation();

            if (id != 0 && id.HasValue)
            {
                objReservation = _unitOfWork.Reservation.GetById(id);
            }

            if (objReservation == null)
            {
                return NotFound();
            }

            objReservation.User = _unitOfWork.ApplicationUser.Get(x => x.Id == objReservation.UserId);
            tempOrderDate = (DateTime)objReservation.OrderDate;
            return Page();
        }

        public IActionResult OnPost()
        {
            System.Diagnostics.Debug.WriteLine("Trigger OnPost");
            /*if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState Invalid?");
                return Page();
            }*/

            if (objReservation.OrderDate == null)
            {
                objReservation.OrderDate = tempOrderDate; // System.DateTime.Now;
                System.Diagnostics.Debug.WriteLine("Warning: No OrderDate ???");
            }

            if (objReservation.Id == 0)
            {
                _unitOfWork.Reservation.Add(objReservation);

                TempData["success"] = "Reservation added successfully";
                System.Diagnostics.Debug.WriteLine("Reservation added successfully");
            }
            else
            {
                _unitOfWork.Reservation.Update(objReservation);
                TempData["success"] = "Reservation updated successfully";
                System.Diagnostics.Debug.WriteLine("Reservation updated successfully");
            }
            _unitOfWork.Commit();

            return RedirectToPage("./Index");
        }
    }
}
