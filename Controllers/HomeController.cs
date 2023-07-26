using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    
    public class HomeController:Controller
    {
        //FromServices. questo  serve a dare la giusta indicazione al model building 
        //che questa istanza deve essere cercata
        //tra i servizi registrati per la dependency injection.
        public async Task<IActionResult> Index([FromServices]  ICachedCourseService courseService){
            ViewData["Title"] = "Benvenuto sul sito";
            List<CourseViewModel> bestRatingCourses =  await courseService.GetBestRatingCoursesAsync();
            List<CourseViewModel> mostRecentCourses = await courseService.GetMostRecentCoursesAsync();

            HomeViewModel viewModel = new HomeViewModel{
                BestRatingCourses = bestRatingCourses,
                MostRecentCourses = mostRecentCourses
            };
            return View(viewModel) ;
        }
    }
}