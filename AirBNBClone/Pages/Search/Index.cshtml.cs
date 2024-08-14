using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace AirBNBClone.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;  //local instance of the database service

        public List<Rental> objRentalList;

        public List<int> objRentalMainImageIdList;

        public List<string> objRentalOwnerPhoneNumberList;

        public List<string> objRentalOwnerNameList;

        public class MegaRentalInfo
        {
            public Rental Rental { get; set; }
            public int ImageId { get; set; }
            public string PhoneNumber { get; set; }
            public string OwnerName { get; set; }
        }

        public List<MegaRentalInfo> objMegaRentalInfoList;

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
        public IActionResult OnGet(string Country, string City, int? Beds, int? Baths)
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

            // now get the primary image for each rental and populate objRentalMainImageIdList

            objRentalMainImageIdList = new List<int>();
            foreach (var rental in objRentalList)
            {
                objRentalMainImageIdList.Add(_unitOfWork.Photo.GetAll().Where(x => x.RentalId == rental.Id && x.PrimaryImage).FirstOrDefault().Id);
            }

            // now populate objRentalOwnerPhoneNumberList
            objRentalOwnerPhoneNumberList = new List<string>();
            objRentalOwnerNameList = new List<string>();
            foreach (var rental in objRentalList)
            {
                var owner = _unitOfWork.ApplicationUser.GetAll().Where(x => x.Id == rental.OwnerId).FirstOrDefault();
                if (owner != null)
                {
                    objRentalOwnerPhoneNumberList.Add(owner.PhoneNumber);
                    objRentalOwnerNameList.Add(owner.FirstName + " " + owner.LastName);
                }
                else
                {
                    // Handle the case where owner is null
                    objRentalOwnerPhoneNumberList.Add("Unknown");
                    objRentalOwnerNameList.Add("Unknown");
                }
            }
            // print the 4 lists to debug console
            System.Diagnostics.Debug.WriteLine("objRentalList");
            foreach (var rental in objRentalList)
            {
                System.Diagnostics.Debug.WriteLine(rental.Id);
            }
            System.Diagnostics.Debug.WriteLine("------");
            foreach (var imageId in objRentalMainImageIdList)
            {
                System.Diagnostics.Debug.WriteLine(imageId);
            }
            System.Diagnostics.Debug.WriteLine("------");
            foreach (var phoneNumber in objRentalOwnerPhoneNumberList)
            {
                System.Diagnostics.Debug.WriteLine(phoneNumber);
            }
            System.Diagnostics.Debug.WriteLine("------");
            foreach (var ownerName in objRentalOwnerNameList)
            {
                System.Diagnostics.Debug.WriteLine(ownerName);
            }
            System.Diagnostics.Debug.WriteLine("------");

            objMegaRentalInfoList = new List<MegaRentalInfo>();


            for (int i = 0; i < objRentalList.Count; i++)
            {
                // assert list[i] exists for all 4 lists (they are not too short)
                if (objRentalMainImageIdList.Count <= i || objRentalOwnerPhoneNumberList.Count <= i || objRentalOwnerNameList.Count <= i)
                {
                    return RedirectToPage("/Error");
                }
                var elem = new MegaRentalInfo
                {
                    Rental = objRentalList[i],
                    ImageId = objRentalMainImageIdList[i],
                    PhoneNumber = objRentalOwnerPhoneNumberList[i],
                    OwnerName = objRentalOwnerNameList[i]
                };
                objMegaRentalInfoList.Add(elem);
            }

            return Page();



        }
    }
}
