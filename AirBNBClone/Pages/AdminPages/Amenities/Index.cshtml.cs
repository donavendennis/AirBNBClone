using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.AdminPages.Amenities
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        public List<Amenity> objAmenityList;

        [BindProperty]
        public int Id { get; set;}

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Description { get; set; }

        public IActionResult OnGet()
        {
            objAmenityList = _unitOfWork.Amenity.GetAll().ToList();
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            System.Diagnostics.Debug.Write("OnPost");
            System.Diagnostics.Debug.Write(id);
            if (id != 0)
            {
                // write to system console we are going to delete id
                System.Diagnostics.Debug.WriteLine("Deleting id: " + id);
                var AmenityToDelete = _unitOfWork.Amenity.GetById(id);
                if (AmenityToDelete != null) { 
                    _unitOfWork.Amenity.Delete(AmenityToDelete);
                    _unitOfWork.Commit();
                }
            }

            return RedirectToPage("./Index");
        }
        public IActionResult OnPostAdd(string Name, string Description)
        {
            System.Diagnostics.Debug.Write("OnPostAdd");
            System.Diagnostics.Debug.Write("Adding: " + Name + " == " + Description);
            if (Name != null && Description != null)
            {
                Amenity objAmenity = new Amenity();
                objAmenity.Name = Name;
                objAmenity.Description = Description;
                _unitOfWork.Amenity.Add(objAmenity);
                _unitOfWork.Commit();
            }
            return RedirectToPage("./Index");
        }
    }
}
