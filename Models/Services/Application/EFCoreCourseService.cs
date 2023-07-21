using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyCourse.Models.Entities;
using MyCourse.Models.Options;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class EFCoreCourseService : ICourseService
    {
        private readonly MyCourseDbContext dbContext;
        public IOptionsMonitor<CoursesOptions> coursesOption { get; }

        public EFCoreCourseService(MyCourseDbContext dbContext, IOptionsMonitor<CoursesOptions> coursesOption)
        {
            this.coursesOption = coursesOption;
            this.dbContext = dbContext;
        }
        public async Task<CourseDetailViewModel> GetCourseByIdAsync(int id)
        {
           IQueryable<CourseDetailViewModel> queryLinq = dbContext.Courses
                .AsNoTracking()
                .Include(course => course.Lessons)
                .Where(course => course.Id == id)
                .Select(course => CourseDetailViewModel.FromEntity(course)); //Usando metodi statici come FromEntity, la query potrebbe essere inefficiente. Mantenere il mapping nella lambda oppure usare un extension method personalizzato
            
            CourseDetailViewModel viewModel = await queryLinq.SingleAsync();
                                                           //.FirstOrDefaultAsync(); //Restituisce null se l'elenco è vuoto e non solleva mai un'eccezione
                                                           //.SingleOrDefaultAsync(); //Tollera il fatto che l'elenco sia vuoto e in quel caso restituisce null, oppure se l'elenco contiene più di 1 elemento, solleva un'eccezione
                                                           //.FirstAsync(); //Restituisce il primo elemento, ma se l'elenco è vuoto solleva un'eccezione
                
            return viewModel;
        }

        public async Task<List<CourseViewModel>> GetCoursesAsync(string search, int page, string orderby, bool ascending)
        {
            /*
            Search che operando alla sua sinistra viene valutato e se vale null viene restituito il valore
            del secondo operando quindi stringa vuota, altrimenti viene restituito il suo stesso valore.
            */
            search = search ?? "";

            page = Math.Max(1, page);
            int limit = this.coursesOption.CurrentValue.PerPage;
            int offset = (page-1) * limit;
            
            var orderOptions = this.coursesOption.CurrentValue.Order;
            if(!orderOptions.Allow.Contains(orderby)){
                orderby = orderOptions.By;
                ascending = orderOptions.Ascending;
            }

            IQueryable<Course> baseQuery = dbContext.Courses;

            switch(orderby)
            {
                case "Title":
                    if (ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.Title);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.Title);
                    }
                    break;
                case "Rating":
                    if (ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.Rating);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.Rating);
                    }
                    break;
                case "CurrentPrice":
                    if (ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.CurrentPrice.Amount);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.CurrentPrice.Amount);
                    }
                    break;
                case "Id":
                    if (ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.Id);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.Id);
                    }
                    break;
            }

            IQueryable<CourseViewModel> queryLinq = baseQuery
            .Skip(offset)
            .Take(limit)
            .Where(course => course.Title.Contains(search))
            .AsNoTracking()
            .Select(course => 
            CourseViewModel.FromEntity(course));

            var courses = await queryLinq.ToListAsync();
            return courses;
        }
    }
}