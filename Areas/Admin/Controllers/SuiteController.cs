using Hotel.Application;
using Hotel.Application.Common.Interfaces;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin_Role)]
    public class SuiteController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public SuiteController(IUnitOfWork uow, IWebHostEnvironment webHostEnvironment)
        {
            _uow = uow;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var suites = _uow.Suite.GetAll();
            return View(suites);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Suite obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The name and description cannot be the same.");
            }

            if (ModelState.IsValid)
            {
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Suite");

                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                    }

                    obj.ImageUrl = "/images/Suite/" + fileName;
                }
                else
                {
                    obj.ImageUrl = "/images/Suite/default.png"; 
                }

                _uow.Suite.Add(obj);
                _uow.Save();
                TempData["success"] = "Suite created successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Error creating suite";
            return View(obj);
        }


        public IActionResult Update(int suiteId)
        {
            Suite? obj= _uow.Suite.Get(u=>u.SuiteId == suiteId);          

            if (obj is null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Update(Suite obj)
        {
            if (ModelState.IsValid && obj.SuiteId>0)
            {
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Suite");

                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('/'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                    }

                    obj.ImageUrl = "/images/Suite/" + fileName;
                }

                _uow.Suite.Update(obj);
                _uow.Save();
                TempData["success"] = "Suite updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error updating suite";
            return View();
        }

        public IActionResult Delete(int suiteId)
        {
            Suite? obj = _uow.Suite.Get(u => u.SuiteId == suiteId);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Suite obj)
        {
            Suite? objFromDb = _uow.Suite.Get(u => u.SuiteId == obj.SuiteId);
            if (objFromDb is not null)
            {
                if (!string.IsNullOrEmpty(obj.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _uow.Suite.Remove(objFromDb);
                _uow.Save();
                TempData["success"] = "Suite deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error deleting suite";
            return View();
        }

    }
}
