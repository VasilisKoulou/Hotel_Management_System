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
    public class SuiteNumberController : Controller
    {
        private readonly IUnitOfWork _uow;


        public SuiteNumberController(IUnitOfWork uow)
        { 
            _uow = uow;
        }
        public IActionResult Index()
        {
            var suiteNumbers = _uow.SuiteNumber.GetAll(includeProperties:"Suite");
            return View(suiteNumbers);
        }

        public IActionResult Create()
        {
            SuiteNumberVM suiteNumberVM = new()
            {
                SuiteList = _uow.Suite.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.SuiteId.ToString()
                })
            }; 

            return View(suiteNumberVM);
        }

        [HttpPost]
        public IActionResult Create(SuiteNumberVM obj)
        {
           // ModelState.Remove("Suite");

            bool suiteNumberExists = _uow.SuiteNumber.Any(u => u.Suite_Number == obj.SuiteNumber.Suite_Number);


            if (ModelState.IsValid && !suiteNumberExists)
            {
                _uow.SuiteNumber.Add(obj.SuiteNumber);
                _uow.Save();
                TempData["success"] = "Suite Number created successfully";
                return RedirectToAction("Index");
            }


            if(suiteNumberExists)
            {
                TempData["error"] = "Error creating suite";
            }

            obj.SuiteList = _uow.Suite.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.SuiteId.ToString()
            });

            return View(obj);
        }

        public IActionResult Update(int suiteNumberId)
        {
            SuiteNumberVM suiteNumberVM = new()
            {
                SuiteList = _uow.Suite.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.SuiteId.ToString()
                }),
                SuiteNumber = _uow.SuiteNumber.Get(u => u.Suite_Number == suiteNumberId)
            };


            if (suiteNumberVM.SuiteNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(suiteNumberVM);
        }
        [HttpPost]
        public IActionResult Update(SuiteNumberVM suiteNumberVM)
        {

            if (ModelState.IsValid)
            {
                _uow.SuiteNumber.Update(suiteNumberVM.SuiteNumber);
                _uow.Save();
                TempData["success"] = "Suite Number updated successfully";
                return RedirectToAction("Index");
            }

            suiteNumberVM.SuiteList = _uow.Suite.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.SuiteId.ToString()
            });

            return View(suiteNumberVM);
        }

        public IActionResult Delete(int suiteNumberId)
        {
            SuiteNumberVM suiteNumberVM = new()
            {
                SuiteList = _uow.Suite.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.SuiteId.ToString()
                }),
                SuiteNumber = _uow.SuiteNumber.Get(u => u.Suite_Number == suiteNumberId)
            };


            if (suiteNumberVM.SuiteNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(suiteNumberVM);
        }
        [HttpPost]
        public IActionResult Delete(SuiteNumberVM suiteNumberVM)
        {
            SuiteNumber? objFromDb = _uow.SuiteNumber.Get(u => u.Suite_Number == suiteNumberVM.SuiteNumber.Suite_Number);
            if (objFromDb is not null)
            {
                _uow.SuiteNumber.Remove(objFromDb);
                _uow.Save();
                TempData["success"] = "Suite Number deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error deleting suite";
            return View();
        }

    }
}
