using System;

namespace Fibonacci.Models
{
    public class HistoricEntryDto
    {
        public long Input { get; set; }

        // Result can be big and long (Int64) can't hold it
        public string Result { get; set; }

        public DateTime DateCreated { get; set; }
    }
}