using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.Reservations
{
    public class ConfirmModel : PageModel

    {
        [BindProperty]

        public string result { get; set; }

        private readonly UnitOfWork _unitOfWork;

        public ConfirmModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            result = "";
        }
        public void OnGet()
        {

            // get the Id
            int Id = int.Parse(Request.Query["id"]);

            // get the Reservation with the Id
            var objReservation = _unitOfWork.Reservation.GetById(Id);

            if (objReservation == null)
            {
                result = "Non-existent booking";
                return;
            }

            objReservation.Confirm = true;

            // update the Reservation
            _unitOfWork.Reservation.Update(objReservation);

            _unitOfWork.Commit();

            result = "Booking confirmed";

        }
    }
}
