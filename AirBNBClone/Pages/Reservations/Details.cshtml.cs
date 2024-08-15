using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;
using System.Security.Claims;
using Utility;

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
        [BindProperty(SupportsGet = true)]
        public DateOnly prefillStartDate { get; set; }


        [BindProperty(SupportsGet = true)]
        public DateOnly prefillEndDate { get; set; }

        [BindProperty]
        public int Id { get; set; } // pass it along

        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
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

        public IActionResult OnPost(int Id, [FromBody] DateOnly prefillStartDate, [FromBody] DateOnly prefillEndDate)
        {

            Id = int.Parse(Request.Query["id"]);
            prefillStartDate = DateOnly.Parse(Request.Query["StartDate"]);
            prefillEndDate = DateOnly.Parse(Request.Query["EndDate"]);



            var PriceSum_final = 0;
            string PriceSumFormula_final = "";

            var objFeeRentalList = _unitOfWork.FeeRental.GetAll().Where(x => x.RentalId == Id).ToList();
            // go through the filteres FeeRental and get the Fee for each one, putting it in objFeeList
            foreach (var feeRental in objFeeRentalList)
            {
                objFeeList.Add(_unitOfWork.Fee.GetById(feeRental.FeeId));
                objFeeAmountList.Add(feeRental.Amount);
                PriceSum_final += feeRental.Amount;
            }
            // redirect to reservation index with the dates
            //Response.Redirect($"/Reservations/Details?StartDate={prefillStartDate}&EndDate={prefillEndDate}&Id={Id}");

            // print the dates to the debug console

            System.Diagnostics.Debug.WriteLine("Start Date: " + prefillStartDate);
            System.Diagnostics.Debug.WriteLine("End Date: " + prefillEndDate);

            var checkingDay = prefillStartDate;

            var separator = "";


            while (checkingDay <= prefillEndDate)
            {

                // first get if there are any reservations
                var objReservation2 = _unitOfWork.Reservation.GetAll().Where(x => x.RentalId == Id && x.Start <= checkingDay && x.End >= checkingDay).FirstOrDefault();
                if (objReservation2 is not null)
                {
                    // add 0 to the list
                    PriceSum_final = -871;
                    PriceSumFormula_final = "Cannot Book! Already Booked on Day " + checkingDay;
                    break;
                }
                // get the price of the day by going through all the prices that start before and end after the checking day, then getting the one with the highest priority
                var objPrice = _unitOfWork.Price.GetAll().Where(x => x.RentalId == Id && x.Start <= checkingDay && x.End >= checkingDay).OrderByDescending(x => x.Priority).FirstOrDefault();
                if (objPrice is not null)
                {
                    // add the price to the list
                    PriceSum_final += objPrice.Amount;
                    totalCost += objPrice.Amount;
                    PriceSumFormula_final += separator + objPrice.Amount;
                    separator = " + ";
                }
                else
                {
                    // add 0 to the list
                    PriceSum_final = -871;
                    PriceSumFormula_final = "Cannot Book! No Price On Day " + checkingDay;
                    break;
                }

                checkingDay = checkingDay.AddDays(1);
            }

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // populate the reservation object and add it to the database

            Reservation objReservation = new Reservation
            {
                RentalId = Id,
                UserId = claim.Value,
                Start = prefillStartDate,
                End = prefillEndDate,
                Confirm = false,
                OrderDate = DateTime.Now,
            };
            _unitOfWork.Reservation.Add(objReservation);

            // get the Id of the reservation just created

            _unitOfWork.Commit();

            var ReservationId = _unitOfWork.Reservation.GetAll().FirstOrDefault(x => x.UserId == claim.Value && x.RentalId == Id && x.Start == prefillStartDate && x.End == prefillEndDate).Id;


            //return RedirectToPage("OrderConfirmation", new { orderId = ShoppingCartVM.OrderHeader.Id });
            //stripe settings 
            var domain = "https://localhost:7080";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"/Reservations/Confirm?Id={ReservationId}",
                CancelUrl = domain + $"/index",
            };

            /*foreach (var item in ShoppingCartVM.cartItems)
            {*/




            var sessionLineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(PriceSum_final * 100),//20.00 -> 2000
                    Currency = "usd",

                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Rental Fee"

                    },

                },
                Quantity = 1,



            };

            options.LineItems.Add(sessionLineItem);

            /*   }    */

            var service = new SessionService();
            Session session = service.Create(options);

            //grab the URL from the session
            Response.Headers.Add("Location", session.Url);
            //return a redirect to the original session location
            _unitOfWork.Reservation.UpdateStripePaymentID(ReservationId, session.Id, session.PaymentLinkId);
            _unitOfWork.Commit();

            return new StatusCodeResult(303);  //success URL



        }
    }
}
