using Microsoft.AspNetCore.Mvc;
using BasicGameService.Models;
using BasicGameService.Models.Stats;

namespace BasicGame.Controllers
{
    public class AdminController : Controller
    {
        // Dashboard: show all devices
        public IActionResult Index()
        {
            var devices = Database.Devices;
            return View(devices);
        }

        // Start session
        [HttpPost]
        public IActionResult StartSession(int deviceId, int? gameId, string playerName)
        {
            var device = Database.Devices.FirstOrDefault(d => d.Id == deviceId);
            if (device == null || !device.IsAvailable)
                return BadRequest("Device not available");

            var session = new Session
            {
                Id = Database.Sessions.Count + 1,
                DeviceId = deviceId,
                GameId = gameId,
                PlayerName = playerName,
                StartTime = DateTime.Now
            };

            Database.Sessions.Add(session);
            device.CurrentSession = session;
            device.IsAvailable = false;

            return RedirectToAction("Index");
        }

        // End session
        [HttpPost]
        public IActionResult EndSession(int sessionId)
        {
            var session = Database.Sessions.FirstOrDefault(s => s.Id == sessionId);
            if (session == null || session.EndTime != null)
                return BadRequest("Invalid session");

            session.EndTime = DateTime.Now;
            var device = Database.Devices.FirstOrDefault(d => d.Id == session.DeviceId);
            if (device != null)
            {
                device.IsAvailable = true;
                device.CurrentSession = null;
            }

            return RedirectToAction("Index");
        }

        // Statistics page
        public IActionResult Statistics()
        {
            var devices = Database.Devices ?? new List<Device>();
            var games = Database.Games ?? new List<Game>();
            var sessions = Database.Sessions ?? new List<Session>();

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
    }
}
