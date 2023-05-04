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
            string query = "SELECT * FROM COURSES";
            DataSet dataSet = db.Query(query);
            throw new NotImplementedException();
        }
    }
}