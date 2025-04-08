using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hotel_Management_System.Controllers
{

    public class SuiteNumberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuiteNumberController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var suiteNumbers = _context.SuiteNumbers.ToList();
            return View(suiteNumbers);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = _context.Suites.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.SuiteId.ToString()
            });

            ViewData["SuiteList"] = list;
            return View();
        }

        [HttpPost]
        public IActionResult Create(SuiteNumber obj)
        {
           // ModelState.Remove("Suite");
            if (ModelState.IsValid)
            {
                _context.SuiteNumbers.Add(obj);
                _context.SaveChanges();
                TempData["success"] = "Suite Number created successfully";
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
