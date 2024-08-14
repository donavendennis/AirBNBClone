using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.Images
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            var image = await _unitOfWork.Photo.GetAsync(x => x.Id == id);
            if (image == null)
            {
                return NotFound();
            }
            return File(image.ImageData, image.ImageType);
        }
    }
}
