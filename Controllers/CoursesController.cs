using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController: Controller
    {
        public IActionResult Index()
        {
            var courseService = new CourseService();
            List<CourseViewModel> courses = courseService.GetCourses();
            return View(courses);
        }
        public IActionResult Detail(int id)
        {
            var courseService = new CourseService();
            CourseDetailViewModel courses = courseService.GetCourse(id);
            return View(courses);
        }
    }
}