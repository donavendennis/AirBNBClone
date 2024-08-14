using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.Reservations
{
    public class DateSelectorModel : PageModel
    {
        // Add the StartDate and EndDate properties
        [BindProperty]
        public DateOnly StartDate { get; set; }
        [BindProperty]
        public DateOnly EndDate { get; set; }

        [BindProperty]
        public int Id { get; set; } // pass it along

        public void OnGet()
        {
            // Initialize the dates if necessary
            StartDate = DateOnly.FromDateTime(DateTime.Now);
            EndDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
            Id = int.Parse(Request.Query["Id"]);
        }

        public void OnPost()
        {
            // redirect to reservation index with the dates
            Response.Redirect($"/Reservations/Details?StartDate={StartDate}&EndDate={EndDate}&Id={Id}");
        }
    }
}
