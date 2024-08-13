using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.Demo.Rentals
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objFeeList = new List<Fee>();
            objFeeRentalList = new List<FeeRental>();
            objFeeAmountList = new List<int>();
            objAmenityList = new List<Amenity>();
            objRentalAmenityList = new List<RentalAmenity>();
        }

        public Rental objRental;
        public List<Fee> objFeeList;
        public List<FeeRental> objFeeRentalList;
        public List<int> objFeeAmountList;

        public List<Amenity> objAmenityList;
        public List<RentalAmenity> objRentalAmenityList;

        public IActionResult OnGet(int id)
        {
            objRental = _unitOfWork.Rental.GetById(id);
            objFeeRentalList = _unitOfWork.FeeRental.GetAll().Where(x => x.RentalId == id).ToList();
            // go through the filteres FeeRental and get the Fee for each one, putting it in objFeeList
            foreach (var feeRental in objFeeRentalList)
            {
                objFeeList.Add(_unitOfWork.Fee.GetById(feeRental.FeeId));
                objFeeAmountList.Add(feeRental.Amount);
            }

            objRentalAmenityList = _unitOfWork.RentalAmenity.GetAll().Where(x => x.RentalId == id).ToList();
            // go through the filteres RentalAmenity and get the Amenity for each one, putting it in objAmenityList
            foreach (var rentalAmenity in objRentalAmenityList)
            {
                objAmenityList.Add(_unitOfWork.Amenity.GetById(rentalAmenity.AmenityId));
            }
            return Page();
        }   

    }
}
