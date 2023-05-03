using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyCourse.Controllers
{
    public class CoursesController: Controller
    {
        public IActionResult Index()
        {
            // return vieW restituisce la view di default chiamata index. Il parametro serve per richiamare una view con nome differente
            return View();
        }
        public IActionResult Detail(string id)
        {
            return View();
        }
    }
}