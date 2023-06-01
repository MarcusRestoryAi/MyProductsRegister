namespace MyProductsRegister.Models
{
    public class ManufacturerProducts
    {
        public ICollection<Products> Products { get; set; }
        public ICollection<Manufacturer> Manufacturer { get; set; }
    }
}
