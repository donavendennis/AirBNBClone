using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.Rentals
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        public IEnumerable<Rental> objRentalList;

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objRentalList = new List<Rental>();
        }

        public IActionResult OnGet(int id)
        {
            objRentalList = _unitOfWork.Rental.GetAll();
            return Page();
        }

    }
}
