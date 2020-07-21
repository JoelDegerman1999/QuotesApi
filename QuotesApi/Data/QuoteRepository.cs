using QuotesApi.Models;
using System.Collections.Generic;

namespace QuotesApi.Data
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly DataContext _context;
        public QuoteRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public void DeleteQuote(int id)
        {
            var quote = GetQuote(id);
            _context.Quotes.Remove(quote);
        }

        public Quote GetQuote(int id)
        {
            return _context.Quotes.Find(id);
        }

        public IEnumerable<Quote> GetQuotes()
        {
            return _context.Quotes;
        }

        public void InsertQuote(Quote quote)
        {
            _context.Quotes.Add(quote);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateQuote(Quote quote)
        {
            _context.Quotes.Update(quote);
        }
    }
}
