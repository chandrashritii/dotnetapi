using dotnetapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Microsoft.VisualBasic;

namespace dotnetapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDbContext _employeeDbContext;
        public EmployeeController(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        [HttpGet]
        [Route("GetEmployee")]
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _employeeDbContext.Employee.ToListAsync();
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<Employee> AddEmployee(Employee objEmployee)
        {
            _employeeDbContext.Employee.Add(objEmployee);
            await _employeeDbContext.SaveChangesAsync();
            return objEmployee;

        }

        [HttpPatch]
        [Route("UpdateEmployee/{id}")]
        public async Task<Employee> UpdateEmployee(Employee objEmployee)
        {
            _employeeDbContext.Entry(objEmployee).State= EntityState.Modified;
            await _employeeDbContext.SaveChangesAsync();
            return objEmployee;
        }

        [HttpDelete]
        [Route("DeleteEmployee/{id}")]
        public bool DeleteEmployee(int id)
        {
            bool a = false;
            var employee = _employeeDbContext.Employee.Find(id);
            if (employee != null)
            {
                a = true;
                _employeeDbContext.Entry(employee).State= EntityState.Deleted;
                _employeeDbContext.SaveChanges();
            }
            else
            {
                a = false;
            }
            return a;
        }
    }

}


