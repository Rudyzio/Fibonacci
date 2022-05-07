using System;
using System.ComponentModel.DataAnnotations;

namespace Fibonacci.Entities
{
    public class HistoricEntry
    {
        [Key]
        public int Id { get; set; }

        public long Input { get; set; }

        // Result can be big and long (Int64) can't hold it
        public string Result { get; set; }

        public DateTime DateCreated { get; set; }
    }
}