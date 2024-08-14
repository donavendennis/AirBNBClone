using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;  //local instance of the database service

        public List<Rental> objRentalList;

        // for filtering
        [BindProperty]
        public string Country { get; set; }
        [BindProperty]
        public string City { get; set; }
        [BindProperty]
        public string StartDate { get; set; }
        [BindProperty]
        public string EndDate { get; set; }
        [BindProperty]
        public int? Beds { get; set; }
        [BindProperty]
        public int? Baths { get; set; }


        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void OnGet(string Country, string City, int? Beds, int? Baths)
        {
            objRentalList = _unitOfWork.Rental.GetAll().ToList();
            // print the content of country to debug console
            System.Diagnostics.Debug.WriteLine("Country");
            System.Diagnostics.Debug.WriteLine(Country);
            System.Diagnostics.Debug.WriteLine("------");


            if (Country is not null)
            {
                // filter objRentalsList by Country
                objRentalList = objRentalList.Where(x => x.Country == Country).ToList();
            }
            if (City is not null)
            {
                // filter objRentalsList by City
                objRentalList = objRentalList.Where(x => x.City == City).ToList();
            }
            if (Beds is not null)
            {
                // filter objRentalsList by Beds
                objRentalList = objRentalList.Where(x => x.Beds >= Beds).ToList();
            }
            if (Baths is not null)
            {
                // filter objRentalsList by Baths
                objRentalList = objRentalList.Where(x => x.Baths >= Baths).ToList();
            }
        }
    }
}
