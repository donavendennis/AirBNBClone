using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.Demo.Discounts
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;  //local instance of the database service
        //private readonly ApplicationDbContext _db;  //local instance of the database service

        public List<Discount> objDiscountList;
        public IndexModel(UnitOfWork unitOfWork)  //dependency injection of the database service
        {
            _unitOfWork = unitOfWork;
            objDiscountList = new List<Discount>();

            // write to system console Init Success
            System.Console.WriteLine("Init Success");
        }

        public IActionResult OnGet()
        /*Here are some common implementations of IActionResult:
        ViewResult: Represents a view result, which renders a view to generate an HTML response to the client.
        JsonResult: Represents a JSON result, which serializes data into JSON format and sends it to the client.
        RedirectResult: This represents a redirection result, which redirects the client to a different URL.
        ContentResult: Represents a content result, which sends raw content (such as text or HTML) to the client.
        FileResult: Represents a file result, which sends a file to the client for download.
        */

        {
            objDiscountList = _unitOfWork.Discount.GetAll().ToList();
            // write to system console Init Success
            System.Console.WriteLine("Just before returning to page");
            return Page();
        }
    }
}
