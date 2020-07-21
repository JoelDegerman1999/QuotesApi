using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteRepository _repository;

        public QuoteController(IQuoteRepository quoteRepository)
        {
            _repository = quoteRepository;
        }
        [HttpGet]
        public IActionResult Get(string sort)
        {
            IEnumerable<Quote> quotes;
            switch (sort)
            {
                case "desc":
                    quotes = _repository.GetQuotes().OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = _repository.GetQuotes().OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = _repository.GetQuotes();
                    break;
            }
            return Ok(quotes);
        }

        [HttpGet("[action]")]
        public IActionResult PagingQuote(int pageNumber, int pageSize)
        {
            var quotes = _repository.GetQuotes();
            return Ok(quotes.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }



        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_repository.GetQuote(id));
        }
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            if (quote.CreatedAt == default(DateTime)) quote.CreatedAt = DateTime.Now;
            _repository.InsertQuote(quote);
            _repository.Save();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            var entity = _repository.GetQuote(id);
            entity.Author = quote.Author;
            entity.Description = quote.Description;
            _repository.Save();
            _repository.UpdateQuote(entity);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.DeleteQuote(id);
            _repository.Save();
            return Ok();
        }
    }
}
