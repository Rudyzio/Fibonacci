using System.Collections.Generic;
using Fibonacci.Entities;
using Fibonacci.Helpers;
using Fibonacci.Parameters;

namespace Fibonacci.Data
{
    public interface IHistoricEntryRepository
    {
        IEnumerable<HistoricEntry> GetAll();
        PagedList<HistoricEntry> GetAll(ResourceParameters parameters);
        HistoricEntry Get(long input);
        void Add(HistoricEntry historicEntry);
        bool Save();
    }
}