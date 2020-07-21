using Microsoft.EntityFrameworkCore;
using QuotesApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuotesApi.Data
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataContext _context;
        public CompanyRepository(DataContext dataxContext)
        {
            _context = dataxContext;
        }
        public IEnumerable<Company> GetCompanies()
        {
            return _context.Companies.Include(c => c.Employees);
        }

        public Company GetCompany(int id)
        {
            return _context.Companies.Include(c => c.Employees).FirstOrDefault(c => c.Id == id);
        }

        public void InsertCompany(Company company)
        {
            _context.Companies.Add(company);
        }

        public void DeleteCompany(int id)
        {
            _context.Companies.Remove(GetCompany(id));
        }

        public void UpdateCompany(Company company)
        {
            _context.Companies.Update(company);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
