using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesApi.Controllers
{
    [Authorize(Roles = "User,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _repository;

        public CompaniesController(ICompanyRepository repository)
        {
            _repository = repository;
        }
        // GET: api/<CompaniesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.GetCompanies());
        }

        // GET api/<CompaniesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_repository.GetCompany(id));
        }

        // POST api/<CompaniesController>
        [HttpPost]
        public IActionResult Post([FromBody] Company company)
        {
            _repository.InsertCompany(company);
            _repository.Save();
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<CompaniesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Company company)
        {
            var entity = _repository.GetCompany(id);
            entity.Name = company.Name;
            _repository.UpdateCompany(entity);
            _repository.Save();
            return NoContent();
        }

        // DELETE api/<CompaniesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.DeleteCompany(id);
            _repository.Save();
            return Ok("Record deleted");
        }
    }
}
