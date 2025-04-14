using Microsoft.AspNetCore.Mvc.Rendering;
using Hotel.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Hotel_Management_System.ViewModels
{
    public class SuiteNumberVM
    {
        public SuiteNumber? SuiteNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? SuiteList { get; set; }
    }
}
