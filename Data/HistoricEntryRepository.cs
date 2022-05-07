using System;
using System.Collections.Generic;
using System.Linq;
using Fibonacci.Entities;
using Fibonacci.Helpers;
using Fibonacci.Parameters;

namespace Fibonacci.Data
{
    public class HistoricEntryRepository : IHistoricEntryRepository
    {
        private readonly ApplicationDbContext _context;

        public HistoricEntryRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(HistoricEntry historicEntry)
        {
            if (historicEntry == null)
            {
                throw new ArgumentNullException(nameof(historicEntry));
            }

            _context.HistoricEntries.Add(historicEntry);
        }

        public bool Exists(long input)
        {
            return _context.HistoricEntries.Any(x => x.Input == input);
        }

        public HistoricEntry Get(long input)
        {
            return _context.HistoricEntries.FirstOrDefault(x => x.Input == input);
        }

        public IEnumerable<HistoricEntry> GetAll()
        {
            return _context.HistoricEntries.ToList();
        }

        public PagedList<HistoricEntry> GetAll(ResourceParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            var query = _context.HistoricEntries as IQueryable<HistoricEntry>;

            if (parameters.SearchNumber > 0)
            {
                query = query.Where(x => x.Input == parameters.SearchNumber);
            }

            query = query.OrderByDescending(x => x.DateCreated);

            return PagedList<HistoricEntry>.Create(query, parameters.PageNumber, parameters.PageSize); 
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}