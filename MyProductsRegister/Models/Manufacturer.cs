using System.ComponentModel.DataAnnotations;

namespace MyProductsRegister.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberEmployees { get; set; }
        //Ref till Products
        public ICollection<Products> Products { get; set; }
    }
}
