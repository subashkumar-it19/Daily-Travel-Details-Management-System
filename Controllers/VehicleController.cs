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
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicle
        public async Task<IActionResult> Index()
        {
            return View(await _context.tbl_Vehicles.ToListAsync());
        }

        // GET: Vehicle/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.tbl_Vehicles
                .FirstOrDefaultAsync(m => m.VehicleId == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicle/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicle/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleId,Unique_Id,VehicleName,VehicleModelName,Icon,VehicleType,Create_By,Create_Date,Update_By,Update_Date,Status,Is_Active")] Vehicle vehicle)
        {
           

            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                vehicle.Unique_Id = "DTM_" + vehicle.VehicleId;
                vehicle.Create_By= User.Identity.Name;
                _context.Update(vehicle);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicle/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.tbl_Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicle/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("VehicleId,VehicleName,Icon,VehicleType,VehicleModelName,Create_By,Update_By,Update_Date,Status,Is_Active")] Vehicle vehicle)
        {
            
            if (id != vehicle.VehicleId)
            {
                return NotFound();
            }

            var updateVehicle = await _context.tbl_Vehicles.FindAsync(id);
            if(updateVehicle==null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    updateVehicle.VehicleName = vehicle.VehicleName;
                    updateVehicle.VehicleModelName = vehicle.VehicleModelName;
                    updateVehicle.VehicleType = vehicle.VehicleType;
                    updateVehicle.Update_Date = DateTime.Now;
                    updateVehicle.Icon = vehicle.Icon;
                    updateVehicle.Update_By = User.Identity.Name;
                    _context.Update(updateVehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.VehicleId))
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
            return View(vehicle);
        }


        // POST: Vehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.tbl_Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.tbl_Vehicles.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.tbl_Vehicles.Any(e => e.VehicleId == id);
        }
    }
}
