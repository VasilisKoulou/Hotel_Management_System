using Hotel.Application;
using Hotel.Application.Common.Interfaces;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Hotel_Management_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin_Role)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _uow;


        public AmenityController(IUnitOfWork uow)
        { 
            _uow = uow;
        }
        public IActionResult Index()
        {
            var amenities = _uow.Amenity.GetAll(includeProperties:"Suite");
            return View(amenities);
        }

        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                SuiteList = _uow.Suite.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.SuiteId.ToString()
                })
            }; 

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
           // ModelState.Remove("Suite");



            if (ModelState.IsValid)
            {
                _uow.Amenity.Add(obj.Amenity);
                _uow.Save();
                TempData["success"] = "Amenity created successfully";
                return RedirectToAction("Index");
            }

            obj.SuiteList = _uow.Suite.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.SuiteId.ToString()
            });

            return View(obj);
        }

        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                SuiteList = _uow.Suite.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.SuiteId.ToString()
                }),
                Amenity = _uow.Amenity.Get(u => u.AmenityID == amenityId)
            };


            if (amenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {

            if (ModelState.IsValid)
            {
                _uow.Amenity.Update(amenityVM.Amenity);
                _uow.Save();
                TempData["success"] = "Amenity updated successfully";
                return RedirectToAction("Index");
            }

            amenityVM.SuiteList = _uow.Suite.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.SuiteId.ToString()
            });

            return View(amenityVM);
        }

        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                SuiteList = _uow.Suite.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.SuiteId.ToString()
                }),
                Amenity = _uow.Amenity.Get(u => u.AmenityID == amenityId)
            };


            if (amenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? objFromDb = _uow.Amenity.Get(u => u.AmenityID == amenityVM.Amenity.AmenityID);
            if (objFromDb is not null)
            {
                _uow.Amenity.Remove(objFromDb);
                _uow.Save();
                TempData["success"] = "Amenity deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error deleting suite";
            return View();
        }

    }
}
