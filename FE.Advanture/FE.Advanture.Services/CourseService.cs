using FE.Advanture.Contract;
using FE.Advanture.Models.EMCS;
using FE.Advanture.Pattern.Services;
using Repository.Pattern.EF.Repositories;

namespace FE.Advanture.Services
{
   
    public class CourseService : Service<Course>, ICourseService
    {
        public CourseService(IRepositoryAsync<Course> repository) : base(repository)
        {
        }
    }
}
