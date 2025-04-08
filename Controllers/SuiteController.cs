using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management_System.Controllers
{

    public class SuiteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuiteController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var suites = _context.Suites.ToList();
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
                _context.Suites.Add(obj);
                _context.SaveChanges();
                TempData["success"] = "Suite created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error creating suite";
            return View();
        }

        public IActionResult Update(int suiteId)
        {
            Suite? obj= _context.Suites.FirstOrDefault(u=>u.SuiteId == suiteId);          

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
                _context.Suites.Update(obj);
                _context.SaveChanges();
                TempData["success"] = "Suite updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error updating suite";
            return View();
        }

        public IActionResult Delete(int suiteId)
        {
            Suite? obj = _context.Suites.FirstOrDefault(u => u.SuiteId == suiteId);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Suite obj)
        {
            Suite? objFromDb = _context.Suites.FirstOrDefault(u => u.SuiteId == obj.SuiteId);
            if (objFromDb is not null)
            {
                _context.Suites.Remove(objFromDb);
                _context.SaveChanges();
                TempData["success"] = "Suite deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error deleting suite";
            return View();
        }

    }
}
