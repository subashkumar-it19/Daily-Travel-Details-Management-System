using System.Configuration;
using System.Security.Claims;
using DailyTravelManagement.Models;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DailyTravelManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public DashboardController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DevConnection");

        }

        public IActionResult Login()
        {
           
            return View();
        }

        public static List<Users> RetrieveUser(string Username, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<Users>("SELECT * FROM tbl_Users WHERE Username = @Username", new { Username }).ToList();
            }
        }
        public IActionResult Register()
        {

            return View();
        }

        // Register 
        [HttpPost]
        public async Task<ActionResult> Register(Users newUser)
        {
            if (ModelState.IsValid)
            {
                
                var existingUser = RetrieveUser(newUser.Username, _connectionString).FirstOrDefault();
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View(newUser);
                }

                
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string insertQuery = "INSERT INTO tbl_Users (EmailId, Username, Password, CreatedBy, CreatedDate) VALUES (@EmailId, @Username, @Password, 'Admin', GETDATE())";
                    await connection.ExecuteAsync(insertQuery, new { newUser.Username, newUser.Password, newUser.EmailId });
                }

               
                return RedirectToAction("Login");
            }

            return View(newUser);
        }

        [HttpPost]
        public async Task<ActionResult> Login(Users user)
        {
            if (ModelState.IsValid)
            {
               
                var retrievedUser = RetrieveUser(user.Username, _connectionString).FirstOrDefault();

                if (retrievedUser != null && retrievedUser.Password == user.Password)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, retrievedUser.Username),
                    new Claim("UserId", retrievedUser.UserId.ToString())
                };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            

            return View(user);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        [Authorize]  
        public ActionResult Dashboard()
        {
            var username = User.Identity.Name;  
            var userId = User.FindFirst("UserId")?.Value;  

            
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

        

            
            List<Travel> SelectedTravels = await _context.tbl_Travels
                .Include(x => x.Vehicle)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate && y.Create_By !=null)
                .ToListAsync();

            
            List<string> dateRange = Enumerable
                .Range(0, (EndDate - StartDate).Days + 1)
                .Select(offset => StartDate.AddDays(offset).ToString("yyyy-MM-dd"))
                .ToList();

            ViewBag.Date = dateRange;

           
            int TotalIncome = (int)SelectedTravels
                .Where(i => i.Vehicle.VehicleType != null)
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString();

            int TotalExpense = (int)SelectedTravels
                .Where(i => i.Vehicle.VehicleType != null)
                .Sum(j => j.MaintenanceCharge + j.FuelCharge + j.DriverCommission);
            ViewBag.TotalExpense = TotalExpense.ToString();

            
            int Balance = TotalIncome - TotalExpense;
            ViewBag.Balance = Balance.ToString();

            // Pie Chart 
            ViewBag.PieChart = SelectedTravels
                .Where(i => i.Vehicle.VehicleType != null)
                .GroupBy(j => j.Vehicle.VehicleId)
                .Select(k => new
                {
                    vehicleTitleWithIcon = k.First().Vehicle.TitleWithModel,
                    amount = k.Sum(j => j.Amount),
                    drivercommission = k.Sum(j => j.DriverCommission),
                    maintenance = k.Sum(j => j.MaintenanceCharge),
                    fuelcharge = k.Sum(j => j.FuelCharge),
                    formatedAmount = k.Sum(j => j.Amount).ToString(),
                })
                .ToList();


            return View();
        }


    }
}
