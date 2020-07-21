using QuotesApi.Models;
using System.Collections.Generic;

namespace QuotesApi.Data
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetEmployees();
        Employee GetEmployee(int id);
        void InsertEmployee(Employee employee);
        void DeleteEmployee(int id);
        void UpdateEmployee(Employee employee);
        void Save();
    }
}
