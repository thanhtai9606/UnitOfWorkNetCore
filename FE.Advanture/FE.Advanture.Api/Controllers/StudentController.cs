using FE.Advanture.Common;
using FE.Advanture.Contract;
using FE.Advanture.Models.EMCS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Pattern.EF.UnitOfWork;

namespace FE.Advanture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private OperationResult operationResult = new OperationResult();
        public StudentController(IStudentService studentService, IUnitOfWorkAsync unitOfWork)
        {
            _studentService = studentService;
            _unitOfWork = unitOfWork;
        }
        [AllowAnonymous]
        [HttpPost, Route("AddStudent")]
        public IActionResult AddStudent(Student student)
        {
            
            try
            {
                _studentService.Add(student);
                int res = _unitOfWork.SaveChanges();
                if(res > 0)
                {
                    operationResult.Success = true;
                    operationResult.Message = "Added new record";
                    operationResult.Caption = "Add complete";
                }
               
            }
            catch (System.Exception ex)
            {
                operationResult.Success = false;
                operationResult.Message = ex.ToString();
                operationResult.Caption = "Add failed!";
            }
            return Ok(operationResult);
        }
        [AllowAnonymous]
        [HttpPost, Route("UpdateStudent")]
        public IActionResult UpdateStudent(Student student)
        {
            try
            {
                _studentService.Update(student);
                int res = _unitOfWork.SaveChanges();
                if (res > 0)
                {
                    operationResult.Success = true;
                    operationResult.Message = "Update success";
                    operationResult.Caption = "Update complete";
                }

            }
            catch (System.Exception ex)
            {
                operationResult.Success = false;
                operationResult.Message = ex.ToString();
                operationResult.Caption = "Update failed!";
            }
            return Ok(operationResult);
        }

        [HttpDelete, Route("DeleteStudent")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                _studentService.Delete(id);
                int res = _unitOfWork.SaveChanges();
                if (res > 0)
                {
                    operationResult.Success = true;
                    operationResult.Message = "Delete success";
                    operationResult.Caption = "Delete complete";
                }

            }
            catch (System.Exception ex)
            {
                operationResult.Success = false;
                operationResult.Message = ex.ToString();
                operationResult.Caption = "Delete failed!";
            }
            return Ok(operationResult);
        }
        [HttpGet, Route("GetStudent")]
        public IActionResult GetStudent() => Ok(_studentService.Queryable());

      
    }
}