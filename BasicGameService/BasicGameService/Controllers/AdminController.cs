using BasicGameService.Data;
using BasicGameService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicGameService.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var devices = await _db.Devices
                .Include(d => d.CurrentSession)
                .Include(d => d.InstalledGames)
                .ToListAsync();

            var games = await _db.Games.ToListAsync();
            ViewBag.AllGames = games;

            return View(devices);
        }

        // Start session
        [HttpPost]
        public async Task<IActionResult> StartSession(int deviceId, int? gameId, string playerName)
        {
            var device = await _db.Devices.FindAsync(deviceId);
            if (device == null || !device.IsAvailable)
                return BadRequest("Device not available");

            var session = new Session
            {
                DeviceId = deviceId,
                GameId = gameId,
                PlayerName = playerName,
                StartTime = DateTime.Now
            };

            _db.Sessions.Add(session);

            device.CurrentSession = session; // optional, tracked automatically
            device.IsAvailable = false;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // End session
        [HttpPost]
        public async Task<IActionResult> EndSession(int sessionId)
        {
            var session = await _db.Sessions.FindAsync(sessionId);
            if (session == null || session.EndTime != null)
                return BadRequest("Invalid session");

            session.EndTime = DateTime.Now;

            var device = await _db.Devices.FindAsync(session.DeviceId);
            if (device != null)
            {
                device.IsAvailable = true;
                device.CurrentSession = null;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
