using Microsoft.AspNetCore.Mvc;
using Shaligram_Recruitment.Fillter;
using Shaligram_Recruitment.Models;
using System.Diagnostics;

namespace Shaligram_Recruitment.Controllers
{
    [TypeFilter(typeof(CustomAuthorizationFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
