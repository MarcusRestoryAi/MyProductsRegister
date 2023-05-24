using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProductsRegister.Data;
using MyProductsRegister.Models;
using System.Diagnostics;

namespace MyProductsRegister.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ApplicationDbContext _context { get; set; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //Kolla om Admin roll finns till DB
            if (!_context.Roles.Any(m => m.Name == "Admin"))
            {
                //Anropa funktion för att populate DB
                await PopulateDB();
            }

            return View();
        }

        private async Task PopulateDB()
        {
            //Starta process för att populate DB
            var roleStore = new RoleStore<IdentityRole>(_context);
            var userStore = new UserStore<IdentityUser>(_context);

            string[] roles = { "Admin", "Moderator", "User" };
            foreach(string role in roles)
            {
                //Skapa en role i DB med namnet frpn 'role' variabel
                roleStore.CreateAsync(new IdentityRole(role)).Wait();

                var newRole = _context.Roles.Where(m => m.Name ==  role).FirstOrDefault();
                newRole.NormalizedName = role.ToUpper();

                _context.Roles.Update(newRole);
                await _context.SaveChangesAsync();
            }

            //Skapa nya Users
            string[] users = { "Marcus", "Edvin" };
            string ePostHandler = "@app.se";

            foreach(string user in users)
            {
                
                //Skapa ett nytt IdentityUser objekt
                var newUser = new IdentityUser();
                newUser.UserName = user + ePostHandler;
                newUser.Email = user + ePostHandler;

                newUser.NormalizedUserName = (user + ePostHandler).ToUpper();
                newUser.NormalizedEmail = (user + ePostHandler).ToUpper();

                newUser.EmailConfirmed = true;

                var password = "12345";
                var hasher = new PasswordHasher<IdentityUser>();
                newUser.PasswordHash = hasher.HashPassword(newUser, password);

                //Add user to DB
                userStore.CreateAsync(newUser).Wait();
            }

            //Koppla rollen Admin till user Marcus
            var adminUser = _context.Users.SingleOrDefault(n => n.UserName == "Marcus@app.se");
            await userStore.AddToRoleAsync(adminUser, "Admin");

            //Lägga till Manufacturers till DB
            await GenerateManufacturers();

            //Lägg till Products till DB
            await GenerateProducts();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task GenerateManufacturers()
        {
            string[,] stuff = {
                { "Volvo", "20" },
                { "Arla", "15" },
                { "Microsoft", "100" }
            };
            /*
            var data = new[] {
                new {
                    name = "Justo Eu Ltd",
                    AntalEmplyees = 36
                },
                new {
                    name = "Urna Nec Luctus Institute",
                    AntalEmplyees = 87
                },
                new {
                    name = "Fusce Fermentum Fermentum Corp.",
                    AntalEmplyees = 32
                },
                new {
                    name = "Eleifend Non Dapibus Corp.",
                    AntalEmplyees = 20
                },
                new {
                    name = "Elit Fermentum Ltd",
                    AntalEmplyees = 16
                },
                new {
                    name = "Vel Arcu Foundation",
                    AntalEmplyees = 19
                },
                new {
                    name = "Lectus Convallis Est Corporation",
                    AntalEmplyees = 46
                },
                new {
                    name = "Ut Limited",
                    AntalEmplyees = 40
                },
                new {
                    name = "Parturient Corp.",
                    AntalEmplyees = 68
                },
                new {
                    name = "Sem Molestie Sodales Foundation",
                    AntalEmplyees = 54
                }
            };
            */
            for (int i = 0; i < 3; i++)
            {
                Manufacturer manufacturer = new Manufacturer
                {
                    Name = stuff[i, 0],
                    NumberEmployees = int.Parse(stuff[i, 1])
                };

                _context.Manufacturer.Add(manufacturer);
                await _context.SaveChangesAsync();

            }
        }

        private async Task GenerateProducts()
        {
            string[,] stuff = {
                { "Volvo", "Röd bil"},
                { "Motorcykel", "Går snabbt och mobilt"},
                { "Mjölk", "Len dryck"},
                { "Fil", "Passar till frukost"},
                { "Windows", "Fungerar ibland"},
                { "Dator", "Kraftfull"},
                { "Telefon", "Liten med stor skärm"},
                { "Leksak", "Rolig, håller i max 3 sekunder" },
                { "Pizza", "Med eller utan annanas" }
            };

            List<Manufacturer> manufacturer = await _context.Manufacturer.ToListAsync();
            Random rng = new Random();
            for (int i = 0; i < 9; i++)
            {
                Products products = new Products
                {
                    Name = stuff[i, 0],
                    Description = stuff[i, 1],
                    Manufacturer = manufacturer[rng.Next(manufacturer.Count)]
                };

                _context.Products.Add(products);
                await _context.SaveChangesAsync();

            }
        }
    }
}