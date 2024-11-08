using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Web.Models
{
    public class Size
    {
        [Key]
        public int SizeID { get; set; }
        [Required, MaxLength(10)]
        public string Name { get; set; }  

        public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}