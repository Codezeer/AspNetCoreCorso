using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.Services.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class AdoDotNetCourseService : ICourseService
    {
        public IDatabaseAccessor db { get; }
        public AdoDotNetCourseService(IDatabaseAccessor db)
        {
            this.db = db;
            
        }
        public CourseDetailViewModel GetCourseById(int id)
        {
            throw new NotImplementedException();
        }

        public List<CourseViewModel> GetCourses()
        {
            string query = "SELECT Id, Title, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM COURSES";
            DataSet dataSet = db.Query(query);
            var datatable = dataSet.Tables[0];
            var courseList = new List<CourseViewModel>();
            foreach(DataRow courseRow in datatable.Rows)
            {
                CourseViewModel course = CourseViewModel.FromDataRow(courseRow);                
                courseList.Add(course);
            }
            return courseList;
        }
    }
}