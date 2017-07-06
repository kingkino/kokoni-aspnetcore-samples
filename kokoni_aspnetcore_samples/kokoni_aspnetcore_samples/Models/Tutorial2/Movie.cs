using System;
using System.ComponentModel.DataAnnotations;

namespace kokoni_aspnetcore_samples.Models.Tutorial2
{
    public class Movie
    {
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Release Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        [Required]
        [Range(0, 999.99)]
        public decimal Price { get; set; }
        public string Rating { get; set; }
    }
}
