using DataAccess;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirBNBClone.Pages.Demo.ImageUpload
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        [BindProperty]
        public Photo objPhoto { get; set; }

        public IndexModel(UnitOfWork unitOfWork)
        {
            objPhoto = new Photo();
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
        }
        public void OnPost()
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
                _unitOfWork.Photo.Update(obj);
            }
            else
            {
                // do nothing
            }



        }
    }
}
