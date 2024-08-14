using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.Reservations
{
    public class IndexModel : PageModel
    {
        // Add the StartDate and EndDate properties
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public void OnGet()
        {
            // Initialize the dates if necessary
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);
        }
    }
}
