using Hotel.Application.Common.Interfaces;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management_System.Controllers
{

    public class SuiteController : Controller
    {
        private readonly IUnitOfWork _uow;
        

        public SuiteController(IUnitOfWork uow)
        {
            _uow = uow;
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
            if(obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The name and description cannot be the same.");
            }
            if (ModelState.IsValid)
            {
                _uow.Suite.Add(obj);
                _uow.Save();
                TempData["success"] = "Suite created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error creating suite";
            return View();
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
