using System.ComponentModel.DataAnnotations;

namespace FetchRewards.Models
{
    // Object for passing in points to spend
    public class Points
    {
        [Required]
        public int points { get; set; }
    }
}
