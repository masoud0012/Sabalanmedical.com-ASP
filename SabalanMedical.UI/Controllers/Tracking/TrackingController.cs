using Microsoft.AspNetCore.Mvc;
using SabalanMedical.Core.Domain.Entities.Tracking;

namespace SabalanMedical.UI.Controllers.Tracking
{
    public class TrackingController : Controller
    {
        [Route("/")]
        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        public IActionResult ProductCategory()
        {
            return View();
        }

        [Route("[action]/{Id}")]//Category
        [HttpGet]
        public IActionResult AddProduct(Guid Id)
        {
            return View();
        }

        [Route("[action]/{Id}")]//Category
        [HttpPost]
        public IActionResult AddProduct(Material request)
        {
            return View();
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddPerson(Guid Id)
        {
            return View();
        }

        [Route("[action]")]//Category
        [HttpPost]
        public IActionResult AddPerson(Person request)
        {
            return View();
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddProcessName(Guid Id)
        {
            return View();
        }

        [Route("[action]")]//Category
        [HttpPost]
        public IActionResult AddProcessName(ProcessName request)
        {
            return View();
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddDevice(Guid Id)
        {
            return View();
        }

        [Route("[action]")]//Category
        [HttpPost]
        public IActionResult AddDevice(Device request)
        {
            return View();
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddProcess(Guid Id)
        {
            return View();
        }

        [Route("[action]")]//Category
        [HttpPost]
        public IActionResult AddProcess(Process request)
        {
            return View();
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddProcessDetail(Guid Id)
        {
            return View();
        }

        [Route("[action]")]//Category
        [HttpPost]
        public IActionResult AddProcessDetail(ProcessDetail request)
        {
            return View();
        }
    }
}
