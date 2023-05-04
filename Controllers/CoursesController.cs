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
            ViewData["Title"] = "Catalogo dei corsi";
            var courseService = new CourseService();
            List<CourseViewModel> courses = courseService.GetCourses();
            return View(courses);
        }
        public IActionResult Detail(int id)
        {
            var courseService = new CourseService();
            CourseDetailViewModel course = courseService.GetCourse(id);
            ViewData["Title"] = course.Title;
            return View(course);
        }
    }
}