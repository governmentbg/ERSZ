using System;
using System.ComponentModel.DataAnnotations;

namespace ERSZ.Infrastructure.Data.Models.Common
{
    public class Counter
    {
        [Key]
        public int Id { get; set; }

        public string Label { get; set; }

        public int CounterType { get; set; }

        public int ResetType { get; set; }

        public int InitValue { get; set; }

        public int Value { get; set; }

        public DateTime LastUsed { get; set; }
    }

    public class CounterValueVM
    {
        public int Value { get; set; }
    }
}
