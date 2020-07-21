using QuotesApi.Models;
using System.Collections.Generic;

namespace QuotesApi.Data
{
    public interface IQuoteRepository
    {
        IEnumerable<Quote> GetQuotes();
        Quote GetQuote(int id);
        void InsertQuote(Quote quote);
        void DeleteQuote(int id);
        void UpdateQuote(Quote quote);
        void Save();
    }
}
