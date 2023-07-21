using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyCourse.Models.Options;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class EFCoreCourseService : ICourseService
    {
        private readonly MyCourseDbContext dbContext;
        public IOptionsMonitor<CoursesOptions> CoursesOption { get; }

        public EFCoreCourseService(MyCourseDbContext dbContext, IOptionsMonitor<CoursesOptions> coursesOption)
        {
            this.CoursesOption = coursesOption;
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

        public async Task<List<CourseViewModel>> GetCoursesAsync(string search, int page)
        {
            /*
            Search che operando alla sua sinistra viene valutato e se vale null viene restituito il valore
            del secondo operando quindi stringa vuota, altrimenti viene restituito il suo stesso valore.
            */
            search = search ?? "";

            page = Math.Max(1, page);
            int limit = this.CoursesOption.CurrentValue.PerPage;
            int offset = (page-1) * limit;

            IQueryable<CourseViewModel> queryLinq = dbContext.Courses
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