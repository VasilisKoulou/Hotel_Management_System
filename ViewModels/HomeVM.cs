using Hotel.Domain.Entities;

namespace Hotel_Management_System.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Suite>? SuiteList { get; set; }
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public int Nights { get; set; }

    }
}
