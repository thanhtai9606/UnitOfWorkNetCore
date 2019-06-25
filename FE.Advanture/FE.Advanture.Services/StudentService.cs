using FE.Advanture.Contract;
using FE.Advanture.Models.EMCS;
using FE.Advanture.Pattern.Services;
using Repository.Pattern.EF.Repositories;

namespace FE.Advanture.Services
{

    public class StudentService : Service<Student>, IStudentService
    {
        public StudentService(IRepositoryAsync<Student> repository) : base(repository)
        {
        }
    }
}
