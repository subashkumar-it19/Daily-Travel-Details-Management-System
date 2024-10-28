using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace DailyTravelManagement.Models
{
    public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base (options)
		{}

		public DbSet<Vehicle> tbl_Vehicles { get; set; }
        public DbSet<Travel> tbl_Travels { get; set; }
    }

  
}

