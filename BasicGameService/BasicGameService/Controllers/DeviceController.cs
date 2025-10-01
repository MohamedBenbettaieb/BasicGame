using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicGameService.Data;
using BasicGameService.Models;
using BasicGameService.Models.Stats;
using BasicGameService.DTOs;

namespace BasicGame.Controllers
{
    public class DeviceController : Controller
    {
        private readonly AppDbContext _db;

        public DeviceController(AppDbContext db)
        {
            _db = db;
        }

        // Dashboard: show all devices
        public async Task<IActionResult> Index()
        {
            var devices = await _db.Devices
                .Include(d => d.InstalledGames)
                .Include(d => d.CurrentSession) // optional if CurrentSession is tracked in DB
                .ToListAsync();

            return View(devices);
        }

        // Statistics page
        public async Task<IActionResult> Statistics()
        {
            var devices = await _db.Devices.ToListAsync();
            var games = await _db.Games.ToListAsync();
            var sessions = await _db.Sessions.ToListAsync();

            var deviceStats = devices.Select(d => new DeviceStat
            {
                DeviceId = d.Id,
                DeviceName = d.Name,
                TimesUsed = sessions.Count(s => s.DeviceId == d.Id),
                TotalTimeMinutes = sessions
                    .Where(s => s.DeviceId == d.Id && s.EndTime.HasValue)
                    .Sum(s => (s.EndTime.Value - s.StartTime).TotalMinutes)
            }).ToList();

            var gameStats = games.Select(g => new GameStat
            {
                GameId = g.Id,
                GameName = g.Name,
                TimesPlayed = sessions.Count(s => s.GameId == g.Id)
            }).ToList();

            var vm = new StatisticsViewModel
            {
                DeviceStats = deviceStats,
                GameStats = gameStats
            };

            return View(vm);
        }
        public IActionResult Create()
        {
            // Pass all games for selection
            ViewBag.AllGames = _db.Games.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DeviceCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllGames = _db.Games.ToList();
                return View(dto);
            }

            var device = new Device
            {
                Name = dto.Name,
                Type = dto.Type,
                Description = dto.Description,
                IsAvailable = true,
                InstalledGames = dto.InstalledGameIds != null
                    ? await _db.Games.Where(g => dto.InstalledGameIds.Contains(g.Id)).ToListAsync()
                    : new List<Game>()
            };

            _db.Devices.Add(device);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var device = await _db.Devices
                .Include(d => d.InstalledGames)
                .FirstOrDefaultAsync(d => d.Id == id.Value);

            if (device == null) return NotFound();

            var dto = new DeviceUpdateDto
            {
                Id = device.Id,
                Name = device.Name,
                Type = device.Type,
                Description = device.Description,
                IsAvailable = device.IsAvailable,
                InstalledGameIds = device.InstalledGames?.Select(g => g.Id).ToList()
            };

            // pass list of all games for the multi-select
            ViewBag.AllGames = await _db.Games.ToListAsync();

            return View(dto);
        }

        // POST: Device/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DeviceUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllGames = await _db.Games.ToListAsync();
                return View(dto);
            }

            var device = await _db.Devices
                .Include(d => d.InstalledGames)
                .FirstOrDefaultAsync(d => d.Id == dto.Id);

            if (device == null) return NotFound();

            // Update scalar properties
            device.Name = dto.Name;
            device.Type = dto.Type;
            device.Description = dto.Description;
            device.IsAvailable = dto.IsAvailable;

            // Update InstalledGames (many-to-many)
            if (dto.InstalledGameIds != null && dto.InstalledGameIds.Count > 0)
            {
                var selectedGames = await _db.Games
                    .Where(g => dto.InstalledGameIds.Contains(g.Id))
                    .ToListAsync();

                // replace the entire collection (EF Core will handle the join table)
                device.InstalledGames.Clear();
                foreach (var g in selectedGames)
                    device.InstalledGames.Add(g);
            }
            else
            {
                // If none selected, clear installed games
                device.InstalledGames.Clear();
            }

            try
            {
                _db.Devices.Update(device);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Devices.AnyAsync(e => e.Id == dto.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}