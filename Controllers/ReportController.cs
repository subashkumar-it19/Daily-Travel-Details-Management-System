using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyTravelManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DailyTravelManagement.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [Authorize]
        public async Task<IActionResult> Vehiclereport()
        {

            return View(await _context.tbl_Vehicles.ToListAsync());
           
        }
        [Authorize]
        public async Task<IActionResult> Travelreport()
        {
            var applicatioDbContext = _context.tbl_Travels.Include(t=>t.Vehicle);
            return View(await applicatioDbContext.ToListAsync());
        }


    }
}

