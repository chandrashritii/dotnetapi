using dotnetapi.Controllers;
using dotnetapi.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace dotnetapi.Tests.Controllers
{
    [TestFixture]
    public class EmployeeControllerTests
    {
        private EmployeeController? _employeeController;
        private Mock<EmployeeDbContext>? _mockDbContext;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = new Mock<EmployeeDbContext>();
            _employeeController = new EmployeeController(_mockDbContext.Object);
        }

        [Test]
        public async Task GetEmployees_ReturnsListOfEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { id = 1, empname = "John Doe" },
                new Employee { id = 2, empname = "Jane Smith" }
            };
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employees.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employees.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employees.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employees.AsQueryable().GetEnumerator());
            _mockDbContext?.Setup(m => m.Employee).Returns(mockDbSet.Object);

            // Act
            var result = await _employeeController.GetEmployees();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("John Doe", result.First().empname);
            Assert.AreEqual("Jane Smith", result.Last().empname);
        }



        [Test]
        public async Task AddEmployee_ValidEmployee_ReturnsAddedEmployee()
        {
            // Arrange
            var employee = new Employee { id = 1, empname = "John Doe" };

            _mockDbContext?.Setup(db => db.Employee.Add(It.IsAny<Employee>())).Callback<Employee>(e => employee = e);

            // Act
            var result = await _employeeController.AddEmployee(employee);

            // Assert
            Assert.AreEqual(employee, result);
        }

        [Test]
        public async Task UpdateEmployee_ValidEmployee_ReturnsUpdatedEmployee()
        {
            // Arrange
            var employeeId = 1;
            var employee = new Employee { id = employeeId, empname = "John Doe", empdepartment = "Sales" };
            _mockDbContext.Setup(db => db.Entry(employee).State).Returns(EntityState.Modified);

            // Act
            var result = await _employeeController.UpdateEmployee(employee);

            // Assert
            Assert.AreEqual(employee, result);
        }

        [Test]
        public void DeleteEmployee_ReturnsFalse_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = 1;
            _mockDbContext?.Setup(m => m.Employee.Find(employeeId)).Returns((Employee)null);

            // Act
            var result = _employeeController?.DeleteEmployee(employeeId);

            // Assert
            Assert.IsFalse(result);
        }

    }
}
