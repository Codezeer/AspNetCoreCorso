using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.InputModels;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController: Controller
    {
        private readonly ICourseService courseService;
        public CoursesController(ICachedCourseService courseService)
        {
            this.courseService = courseService;
            
        }
        public async Task<IActionResult> Index(CourseListInputModel input)
        {
            ViewData["Title"] = "Catalogo dei corsi";
            ListViewModel<CourseViewModel> courses = await courseService.GetCoursesAsync(input);
            
            CourseListVewModel viewModel = new CourseListVewModel{
                Courses = courses
                ,Input = input
            };
            return View(viewModel);
        }
        public async Task<IActionResult> Detail(int id)
        {
            CourseDetailViewModel course = await courseService.GetCourseByIdAsync(id);
            ViewData["Title"] = course.Title;
            return View(course);
        }
    }
}