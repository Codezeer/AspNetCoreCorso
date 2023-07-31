using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.ViewModels;

namespace MyCourse.Customizations.ViewComponents
{
    [ViewComponent("Pagination")]
    public class PaginationBarViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(CourseListVewModel model)
        {
            //Numero di pagina corrente
            //Numero di risultati totali
            //Numero di risultati per pagina
            return View(model);
        }
    }
}