using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.Reservations
{
    public class DetailsModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        // Add the StartDate and EndDate properties
        public DetailsModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public DateOnly prefillStartDate { get; set; }


        [BindProperty]
        public DateOnly prefillEndDate { get; set; }

        [BindProperty]
        public int Id { get; set; } // pass it along

        [BindProperty]
        public DateOnly FirstName { get; set; }
        [BindProperty]
        public DateOnly LastName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string State { get; set; }
        [BindProperty]
        public string Zip { get; set; }

        public int PriceSum { get; set; }
        public string PriceSumFormula { get; set; }

        public List<Fee> objFeeList = new List<Fee>();
        public List<int> objFeeAmountList = new List<int>();

        public int totalCost { get; set; }



        public void OnGet()
        {
            Id = int.Parse(Request.Query["id"]);
            prefillStartDate = DateOnly.Parse(Request.Query["StartDate"]);
            prefillEndDate = DateOnly.Parse(Request.Query["EndDate"]);

            var objFeeRentalList = _unitOfWork.FeeRental.GetAll().Where(x => x.RentalId == Id).ToList();
            // go through the filteres FeeRental and get the Fee for each one, putting it in objFeeList
            foreach (var feeRental in objFeeRentalList)
            {
                objFeeList.Add(_unitOfWork.Fee.GetById(feeRental.FeeId));
                objFeeAmountList.Add(feeRental.Amount);
                totalCost += feeRental.Amount;
            }

            // go through each day from start to end, then get the price for that day
            var checkingDay = prefillStartDate;

            var separator = "";

            while (checkingDay <= prefillEndDate)
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
                    totalCost += objPrice.Amount;
                    PriceSumFormula += separator + objPrice.Amount;
                    separator = " + ";
                }
                else
                {
                    // add 0 to the list
                    PriceSum = -871;
                    PriceSumFormula = "Cannot Book! No Price On Day " + checkingDay;
                    break;
                }

                checkingDay = checkingDay.AddDays(1);
            }


        }

        public void OnPost()
        {
            // redirect to reservation index with the dates
            Response.Redirect($"/Reservations/Details?StartDate={prefillStartDate}&EndDate={prefillEndDate}&Id={Id}");
        }
    }
}
