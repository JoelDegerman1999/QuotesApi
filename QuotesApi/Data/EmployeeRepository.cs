using Microsoft.EntityFrameworkCore;
using QuotesApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuotesApi.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _dataContext;

        public EmployeeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _dataContext.Employees.Include(e => e.Company);
        }

        public Employee GetEmployee(int id)
        {
            var entity = _dataContext.Employees.FirstOrDefault(e => e.Id == id);
            if (entity == null) return null;

            _dataContext.Entry(entity)
                .Reference(e => e.Company).Load();
            return entity;
        }

        public void InsertEmployee(Employee employee)
        {
            _dataContext.Employees.Add(employee);
        }

        public void DeleteEmployee(int id)
        {
            _dataContext.Employees.Remove(GetEmployee((id)));
        }

        public void UpdateEmployee(Employee employee)
        {
            _dataContext.Employees.Update(employee);
        }

        public void Save()
        {
            _dataContext.SaveChanges();
        }
    }
}