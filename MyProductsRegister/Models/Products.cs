using System.ComponentModel.DataAnnotations;

namespace MyProductsRegister.Models
{
    public class Products
    {
        //Skapar propertes som senare blir kolumner i tabell
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //Skapa ref tillbaka till Manufacturer
        public Manufacturer Manufacturer { get; set; }

    }
}
