using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirBNBClone.Pages.Rentals
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        //helps us map the physical path to the wwwroot folder on the server amongst other things

        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public Rental objRental { get; set; }

        public IEnumerable<SelectListItem> AmenitiesList { get; set; }
        public IEnumerable<SelectListItem> FeesList { get; set; }
        public Photo objPhoto { get; set; }

        public UpsertModel(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            objRental = new Rental();
            objPhoto = new Photo();
        }

        public IActionResult OnGet(int? id)
        {
            objRental = new Rental();
            AmenitiesList = _unitOfWork.Amenity.GetAll()
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }
                );
            FeesList = _unitOfWork.Fee.GetAll()
                .Select(m => new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                }
                );

            if (id == null || id == 0) //create mode
            {
                return Page();
            }

            //edit mode
            if (id != 0)  //retreive product from DB
            {
                objRental = _unitOfWork.Rental.GetById(id);
            }

            if (objRental == null) //maybe nothing returned
            {
                return NotFound();
            }
            return Page();
        }

        public void uploadPhoto(int rentalId)
        {

            var files = HttpContext.Request.Form.Files;

            // print the length of files to debug console
            System.Diagnostics.Debug.WriteLine(files.Count);

            var myTempStream = new MemoryStream();

            files[0].CopyTo(myTempStream);

            objPhoto.ImageData = myTempStream.ToArray();

            objPhoto.ImageType = files[0].ContentType;

            // check if the current id is in the database by attempting a GetById
            var obj = _unitOfWork.Photo.GetById(objPhoto.Id);

            // check if obj is null

            if (obj is not null)
            {
                // it worked, so we need to delete this image with this Id
                obj.ImageData = objPhoto.ImageData;
                obj.ImageType = objPhoto.ImageType;
                obj.RentalId = rentalId;
                _unitOfWork.Photo.Update(obj);
            }
        }
        
        public IActionResult OnPost()
        {
            var files = HttpContext.Request.Form.Files;

            // Check if the product is new (create)
            if (objRental.Id == 0)
            {
                // Add the new product to the database
                _unitOfWork.Rental.Add(objRental);
            }
            else
            {
                // Update the existing product in the database
                _unitOfWork.Rental.Update(objRental);
            }
            if (files.Count > 0)
            {
                uploadPhoto(objRental.Id);
            }

            // Save changes to the database
            _unitOfWork.Commit();

            // Redirect to the Products Page
            return RedirectToPage("./Index");
        }
    }
}

