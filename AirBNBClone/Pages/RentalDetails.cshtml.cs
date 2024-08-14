using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages
{
    public class RentalDetailsModel : PageModel
    {
        // slight reminder: inherit most of the code of demo/rentals/index.cshtml.cs
        private readonly UnitOfWork _unitOfWork;

        public RentalDetailsModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objFeeList = new List<Fee>();
            objFeeRentalList = new List<FeeRental>();
            objFeeAmountList = new List<int>();
            objAmenityList = new List<Amenity>();
            objRentalAmenityList = new List<RentalAmenity>();
            objPhotoIdList = new List<int>();
            objMainPhotoId = 0;
            objDiscountList = new List<Discount>();
            objAvailabilityStringList = new List<String>();
            objAvailabilityDateList = new List<DateOnly>();
        }

        public Rental objRental;
        public List<Fee> objFeeList;
        public List<FeeRental> objFeeRentalList;
        public List<int> objFeeAmountList;

        public List<Amenity> objAmenityList;
        public List<RentalAmenity> objRentalAmenityList;
        public List<int> objPhotoIdList;

        public int objMainPhotoId = 0;

        public List<Discount> objDiscountList;

        public List<String> objAvailabilityStringList;
        public List<DateOnly> objAvailabilityDateList;

        public int PriceSum;
        public String PriceSumFormula;

        public bool havePriceInfo = false;

        // for submission of start and end for price calculation
        [BindProperty]
        public int Id { get; set; }
        public DateOnly? QueryStart { get; set; }

        public DateOnly? QueryEnd { get; set; }

        public int Id_ReEnter { get; set; }


        public IActionResult OnGet(int Id, DateOnly? QueryStart, DateOnly? QueryEnd)
        {
            Id_ReEnter = Id;
            objRental = _unitOfWork.Rental.GetById(Id);
            objFeeRentalList = _unitOfWork.FeeRental.GetAll().Where(x => x.RentalId == Id).ToList();
            // go through the filteres FeeRental and get the Fee for each one, putting it in objFeeList
            foreach (var feeRental in objFeeRentalList)
            {
                objFeeList.Add(_unitOfWork.Fee.GetById(feeRental.FeeId));
                objFeeAmountList.Add(feeRental.Amount);
            }

            objRentalAmenityList = _unitOfWork.RentalAmenity.GetAll().Where(x => x.RentalId == Id).ToList();
            // go through the filteres RentalAmenity and get the Amenity for each one, putting it in objAmenityList
            foreach (var rentalAmenity in objRentalAmenityList)
            {
                objAmenityList.Add(_unitOfWork.Amenity.GetById(rentalAmenity.AmenityId));
            }

            // now for Photo
            // first get the singular main photo
            objMainPhotoId = _unitOfWork.Photo.GetAll().FirstOrDefault(x => x.RentalId == Id && x.PrimaryImage, new Photo()).Id;

            // then get the rest of the photos
            objPhotoIdList = _unitOfWork.Photo.GetAll().Where(x => x.RentalId == Id && !x.PrimaryImage).Select(x => x.Id).ToList();

            objDiscountList = _unitOfWork.Discount.GetAll().ToList().Where(x => x.RentalId == Id).ToList();

            //iterate from today to 30 days from now

            // get the current date
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            for (int i = 0; i < 30; i++)
            {
                // get the current date plus i days
                DateOnly futureDate = currentDate.AddDays(i);

                objAvailabilityDateList.Add(futureDate);

                // check if the future date is in the reservation list
                var objReservation = _unitOfWork.Reservation.GetAll().Where(x => x.RentalId == Id && x.Start <= futureDate && x.End >= futureDate).FirstOrDefault();

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
                    var objPrice = _unitOfWork.Price.GetAll().Where(x => x.RentalId == Id && x.Start <= futureDate && x.End >= futureDate).OrderByDescending(x => x.Priority).FirstOrDefault();
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
            // print QueryStart and QueryEnd to the debug console as text
            Console.WriteLine("QueryStart: " + QueryStart);
            Console.WriteLine("QueryEnd: " + QueryEnd);

            PriceSumFormula = QueryStart + " to " + QueryEnd + ": ";

            if (QueryStart is not null && QueryEnd is not null && QueryStart <= QueryEnd)
            {
                havePriceInfo = true;

                // go through each day from start to end, then get the price for that day
                var checkingDay = QueryStart;

                while (checkingDay <= QueryEnd)
                {
                    // first get if there are any reservations
                    var objReservation = _unitOfWork.Reservation.GetAll().Where(x => x.RentalId == Id && x.Start <= checkingDay && x.End >= checkingDay).FirstOrDefault();
                    if (objReservation is not null)
                    {
                        // add 0 to the list
                        PriceSum = -871;
                        PriceSumFormula = "Cannot Book! Already Booked on Day " + checkingDay;
                        break;
                    }
                    // get the price of the day by going through all the prices that start before and end after the checking day, then getting the one with the highest priority
                    var objPrice = _unitOfWork.Price.GetAll().Where(x => x.RentalId == Id && x.Start <= checkingDay && x.End >= checkingDay).OrderByDescending(x => x.Priority).FirstOrDefault();
                    if (objPrice is not null)
                    {
                        // add the price to the list
                        PriceSum += objPrice.Amount;
                        PriceSumFormula += objPrice.Amount + " + ";
                    }
                    else
                    {
                        // add 0 to the list
                        PriceSum = -871;
                        PriceSumFormula = "Cannot Book! No Price On Day " + checkingDay;
                        break;
                    }

                    checkingDay = checkingDay.Value.AddDays(1);
                }
            }
            else { havePriceInfo = false; }

            return Page();


        }

    }
}

