using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController: Controller
    {
        private readonly ICachedCourseService courseService;
        public CoursesController(ICachedCourseService courseService)
        {
            this.courseService = courseService;
            
        }
        public async Task<IActionResult> Index(string search, int page, string orderby, bool ascending)
        {
            ViewData["Title"] = "Catalogo dei corsi";
            List<CourseViewModel> courses = await courseService.GetCoursesAsync();
            return View(courses);
        }
        public async Task<IActionResult> Detail(int id)
        {
            CourseDetailViewModel course = await courseService.GetCourseByIdAsync(id);
            ViewData["Title"] = course.Title;
            return View(course);
        }
    }
}