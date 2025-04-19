using System.Diagnostics;
using Hotel.Application.Common.Interfaces;
using Hotel_Management_System.Models;
using Hotel_Management_System.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management_System.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM home = new()
            {
                SuiteList = _unitOfWork.Suite.GetAll(includeProperties:"Suite_Amenity"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now)
            };
            return View(home);
        }
        //[HttpPost]
        //public IActionResult Index(HomeVM home)
        //{
        //    DateOnly checkOutDate = home.CheckInDate.Value.AddDays(home.Nights);

        //    var suites = _unitOfWork.Suite.GetAll(includeProperties: "Suite_Amenity")
        //        .Where(s => !_unitOfWork.Booking
        //            .Any(b => b.SuiteId == s.SuiteId && b.CheckInDate < checkOutDate && b.CheckOutDate > home.CheckInDate)).ToList();

        //    return View(home);
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
