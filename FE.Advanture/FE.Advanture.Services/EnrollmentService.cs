using FE.Advanture.Contract;
using FE.Advanture.Models.EMCS;
using FE.Advanture.Pattern.Services;
using Repository.Pattern.EF.Repositories;

namespace FE.Advanture.Services
{
   
    public class EnrollmentService : Service<Enrollment>, IEnrollmentService
    {
        public EnrollmentService(IRepositoryAsync<Enrollment> repository) : base(repository)
        {
        }
    }
}
