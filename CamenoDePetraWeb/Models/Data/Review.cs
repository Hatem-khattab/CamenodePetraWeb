
 using System;
 using System.ComponentModel.DataAnnotations;

namespace CamenoDePetraWeb.Models.Data
{
    public class Review
        {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "Name is required")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
            [Display(Name = "First Name")]
            public required string Name { get; set; } // اسم الشخص اللي كتب الريفيو

           [Required(ErrorMessage = "Email is required")]
           [EmailAddress(ErrorMessage = "Invalid email address")]
           [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
           [Display(Name = "Email Address")]
           public required string Email { get; set; } // ايميل الشخص (اختياري)

           [Required(ErrorMessage = "Review message is required")]
           [StringLength(1000, MinimumLength = 10, ErrorMessage = "Review must be between 10 and 1000 characters")]
           [Display(Name = "Your Review")]
           public required string Message { get; set; } // نص الريفيو

           [Required(ErrorMessage = "Rating is required")]
          [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
          [Display(Name = "Rating")] 
         public int Rating { get; set; } // تقييم من 1 الى 5

        [DataType(DataType.DateTime)]

        public DateTime CreatedAt { get; set; } = DateTime.Now; // وقت إضافة الريفيو
        }
    

}
