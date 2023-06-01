using FakeItEasy;
using Microsoft.EntityFrameworkCore.Metadata;
using MyProductsRegister.Controllers;
using MyProductsRegister.Data;
using MyProductsRegister.Models;

namespace MyProductsRegisterTest
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            //S�tta upp vilkoren f�r testet
            string prodName = "Dator";
            var fakeProd = A.CollectionOfDummy<Products>(5).AsEnumerable();
            var dataStore = A.Fake<ApplicationDbContext>();
            var controller = new ProductsController(dataStore);
            

            //Utf�ra testet
            Products product = await controller.FetchProductByName( prodName );

            //Assert
            Assert.Equal( prodName, product.Name );

        }
    }
}