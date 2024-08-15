using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Models;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using System.Security.Claims;
using Utility;
using Microsoft.AspNetCore.Mvc;

namespace AirBNBClone.Pages.AdminPages.ReservationAdmin
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Reservation> Reservations { get; set; }

        public string IndicatorString = "";

        public async Task OnGetAsync()
        {

            Reservations = new List<Reservation>();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim is null)
            {
                // redirect to the login page
                // /Identity/Account/Login
                Response.Redirect("/Identity/Account/Login");
                return;
            }

            var currentUserId = claim.Value;


            var Reservations_prefilter = (await _unitOfWork.Reservation.GetAllAsync()).ToList();

            if (User.IsInRole(SD.RenterRole))
            {
                // filter reservations to only show for which RenterId is the current user Id
                Reservations = Reservations_prefilter.Where(x => x.UserId == currentUserId).ToList();
                IndicatorString = "Managing my own reservations as a renter";
            }

            else if (User.IsInRole(SD.OwnerRole))
            {
                // filter reservations to only show for which OwnerId is the current user Id
                foreach (var reservation in Reservations_prefilter)
                {
                    var rental = await _unitOfWork.Rental.GetAsync(x => x.Id == reservation.RentalId);
                    // print renter OwnerId and currentUserId to console
                    System.Diagnostics.Debug.WriteLine("rental.OwnerId: " + rental.OwnerId);
                    System.Diagnostics.Debug.WriteLine("currentUserId: " + currentUserId);
                    System.Diagnostics.Debug.WriteLine("---");
                    if (rental.OwnerId == currentUserId)
                    {
                        Reservations.Add(reservation);
                    }
                }
                IndicatorString = "Managing reservations for which the rental property is owned by me as a renter";
            }
            else if (User.IsInRole(SD.AdminRole))
            {
                Reservations = Reservations_prefilter;
                IndicatorString = "Managing all reservations as a site admin";
            }
            else
            {                 
                // redirect to the login page
                // /Identity/Account/Login
                Response.Redirect("/Identity/Account/Login");
                return;
            }

            foreach (var reservation in Reservations)
            {
                reservation.User = await _unitOfWork.ApplicationUser.GetAsync(x => x.Id == reservation.UserId);
            }
        }
    }
}