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
            objPhotoList = new List<Photo>();
            objDiscountList = new List<Discount>();
            objAvailabilityStringList = new List<String>();
            objAvailabilityDateTimeList = new List<DateTime>();
        }

        public Rental objRental;
        public List<Fee> objFeeList;
        public List<FeeRental> objFeeRentalList;
        public List<int> objFeeAmountList;

        public List<Amenity> objAmenityList;
        public List<RentalAmenity> objRentalAmenityList;
        public List<Photo> objPhotoList;

        public List<Discount> objDiscountList;

        public List<String> objAvailabilityStringList;
        public List<DateTime> objAvailabilityDateTimeList;

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

            // now for Photo
            objPhotoList = _unitOfWork.Photo.GetAll().Where(x => x.RentalId == id).ToList();

            objDiscountList = _unitOfWork.Discount.GetAll().ToList().Where(x => x.RentalId == id).ToList();

            //iterate from today to 30 days from now

            // get the current date
            DateTime currentDate = DateTime.Today;

            for (int i = 0; i < 30; i++)
            {
                // get the current date plus i days
                DateTime futureDate = currentDate.AddDays(i);

                objAvailabilityDateTimeList.Add(futureDate);

                // check if the future date is in the reservation list
                var objReservation = _unitOfWork.Reservation.GetAll().Where(x => x.RentalId == id && x.Start <= futureDate && x.End >= futureDate).FirstOrDefault();

                // if it is, then we need to remove it from the list of available dates
                if (objReservation is not null)
                {
                    // check if the reservation is confirmed
                    if (objReservation.Confirm)
                    {
                        // add string Unavailable to the list
                        objAvailabilityStringList.Add("Already booked and confirmed");
                    }
                    else
                    {
                        // add string Available to the list
                        objAvailabilityStringList.Add("Booking in progress, awaiting confirmation");

                    }
                }
                else
                {
                    // get all prices which is belonging to this day with the highest Priority
                    var objPrice = _unitOfWork.Price.GetAll().Where(x => x.RentalId == id && x.Start <= futureDate && x.End >= futureDate).OrderByDescending(x => x.Priority).FirstOrDefault();
                    if (objPrice is not null)
                    {
                        // add string Available to the list
                        objAvailabilityStringList.Add("Available at a price of " + objPrice.Amount);
                    }
                    else
                    {
                        // add string Available to the list
                        objAvailabilityStringList.Add("Price TBD by owner");
                    }

                }
            }



            return Page();


        }

    }
}