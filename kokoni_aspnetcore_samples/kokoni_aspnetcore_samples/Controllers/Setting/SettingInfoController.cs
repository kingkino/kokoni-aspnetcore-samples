using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace kokoni_aspnetcore_samples.Controllers.Setting
{
    public class SettingInfoController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public SettingInfoController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            string settings = string.Empty;

            settings += _hostingEnvironment.ApplicationName + Environment.NewLine;
            settings += _hostingEnvironment.EnvironmentName + Environment.NewLine;
            settings += _hostingEnvironment.WebRootPath + Environment.NewLine;
            settings += _hostingEnvironment.ContentRootPath + Environment.NewLine;

            ViewData["setting"] = settings;

            return View();
        }
    }
}
