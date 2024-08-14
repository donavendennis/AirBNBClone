using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.AdminPages.Fees
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        public List<Fee> objFeeList;

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
            objFeeList = _unitOfWork.Fee.GetAll().ToList();
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
                var FeeToDelete = _unitOfWork.Fee.GetById(id);
                if (FeeToDelete != null) { 
                    _unitOfWork.Fee.Delete(FeeToDelete);
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
                Fee objFee = new Fee();
                objFee.Name = Name;
                objFee.Description = Description;
                _unitOfWork.Fee.Add(objFee);
                _unitOfWork.Commit();
            }
            return RedirectToPage("./Index");
        }
    }
}
