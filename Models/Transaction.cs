using System;
using System.ComponentModel.DataAnnotations;

namespace FetchRewards.Models
{
    // Represents each transaction
    public record Transaction
    {
        [Required]
        public string Payer { get; set; }

        [Required]
        public int Points { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
