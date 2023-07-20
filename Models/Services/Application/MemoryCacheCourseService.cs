using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class MemoryCacheCourseService : ICachedCourseService
    {
        private readonly IMemoryCache memoryCache;
        private readonly ICourseService courseService;
        public MemoryCacheCourseService(ICourseService courseService, IMemoryCache memoryCache )
        {
            this.courseService = courseService;
            this.memoryCache = memoryCache;
            
        }
        public Task<CourseDetailViewModel> GetCourseByIdAsync(int id)
        {
            return memoryCache.GetOrCreateAsync($"Course{id}", cacheEntry =>{
                cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                return courseService.GetCourseByIdAsync(id);
            });
        }

        public Task<List<CourseViewModel>> GetCoursesAsync()
        {
            return memoryCache.GetOrCreateAsync($"Courses", cacheEntry =>{
                cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                return courseService.GetCoursesAsync();
            });
        }
    }
}