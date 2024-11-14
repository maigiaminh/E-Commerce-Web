using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Web.Models
{
    public class ProductSize
    {
        [Key, Column(Order = 0)]
        public int ProductID { get; set; }

        [Key, Column(Order = 1)]
        public int SizeID { get; set; }

        public int Stock { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [ForeignKey("SizeID")]
        public virtual Size Size { get; set; }
    }
}