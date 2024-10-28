using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DailyTravelManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace DailyTravelManagement.Controllers
{
    public class TravelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TravelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Travel
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.tbl_Travels.Include(t => t.Vehicle);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Travel/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travel = await _context.tbl_Travels
                .Include(t => t.Vehicle)
                .FirstOrDefaultAsync(m => m.TravelId == id);
            if (travel == null)
            {
                return NotFound();
            }

            return View(travel);
        }

        // GET: Travel/Create
        [Authorize]
        public IActionResult Create()
        {
            // ViewData["VehicleId"] = new SelectList(_context.tbl_Vehicles, "VehicleId", "VehicleId");
            PopulateVehicles();
            return View();
        }

        // POST: Travel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TravelId,Unique_Id,VehicleId,Kilometer,Amount,FuelCharge,MaintenanceCharge,DriverCommission,Date,Pickup,Drop,Note,Create_By,Create_Date,Update_By,Update_Date,Status,Is_Active")] Travel travel)
        {
         
            if (ModelState.IsValid)
            {
                _context.Add(travel);
                await _context.SaveChangesAsync();
                travel.Unique_Id = "DTM_" + travel.TravelId;
                travel.Create_By = User.Identity.Name;
                _context.Update(travel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehicleId"] = new SelectList(_context.tbl_Vehicles, "VehicleId", "VehicleId", travel.VehicleId);
            return View(travel);
        }

        // GET: Travel/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var travel = await _context.tbl_Travels.FindAsync(id);
            if (travel == null)
            {
                return NotFound();
            }
            //ViewData["VehicleId"] = new SelectList(_context.tbl_Vehicles, "VehicleId", "VehicleId", travel.VehicleId);
            PopulateVehicles();
            return View(travel);
        }

        // POST: Travel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TravelId,Unique_Id,VehicleId,Kilometer,Amount,FuelCharge,MaintenanceCharge,DriverCommission,Date,Pickup,Drop,Note,Create_By,Create_Date,Update_By,Update_Date,Status,Is_Active") ] Travel travel)
        {
            if (id != travel.TravelId)
            {
                return NotFound();
            }

            var updateTravel = await _context.tbl_Travels.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    updateTravel.Date = travel.Date;
                    updateTravel.VehicleId = travel.VehicleId;
                    updateTravel.Kilometer = travel.Kilometer;
                    updateTravel.Amount = travel.Amount;
                    updateTravel.FuelCharge = travel.FuelCharge;
                    updateTravel.MaintenanceCharge = travel.MaintenanceCharge;
                    updateTravel.DriverCommission = travel.DriverCommission;
                    updateTravel.Pickup = travel.Pickup;
                    updateTravel.Note = travel.Note;

                    updateTravel.Update_Date = DateTime.Now;
                    updateTravel.Update_By = User.Identity.Name;




                    _context.Update(updateTravel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TravelExists(travel.TravelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehicleId"] = new SelectList(_context.tbl_Vehicles, "VehicleId", "VehicleId", travel.VehicleId);
            return View(travel);
        }

      
        // POST: Travel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var travel = await _context.tbl_Travels.FindAsync(id);
            if (travel != null)
            {
                _context.tbl_Travels.Remove(travel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TravelExists(int id)
        {
            return _context.tbl_Travels.Any(e => e.TravelId == id);
        }

        [NonAction]
        public void PopulateVehicles()
        {
            var VehicleCollection = _context.tbl_Vehicles.ToList();
            var vehicleList = VehicleCollection.Select(v =>
                                    new SelectListItem
                                    {
                                        Value = v.VehicleId.ToString(),
                                        Text = v.TitleWithModel
                                    }).ToList();
            vehicleList.Insert(0, new SelectListItem { Value = "", Text = "Choose a Vehicle" });
            ViewBag.Vehicles = vehicleList;
        }

    }
}
