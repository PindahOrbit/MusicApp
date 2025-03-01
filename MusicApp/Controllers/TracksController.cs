using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicApp.Areas.Identity.Data;
using MusicApp.Data;
using MusicApp.Models;

namespace MusicApp.Controllers
{

    [Authorize]
    public class TracksController : Controller
    {
        private readonly MusicDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<MusicAppUser> _userManager;
        public TracksController(MusicDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<MusicAppUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: Tracks
        public async Task<IActionResult> Index()
        {
            var musicDbContext = _context.Tracks.Include(t => t.User);
            return View(await musicDbContext.ToListAsync());
        }

        // GET: Tracks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // GET: Tracks/Create
        public async Task<IActionResult> Create()
        {
            return View(new Track() { UserId = (await _userManager.GetUserAsync(User)).Id});
        }

        // POST: Tracks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(  Track track)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(track);
            }

            var file = Request.Form.Files.FirstOrDefault();

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("FilePath", "Please upload an audio file.");
                return View(track);
            }

            // Ensure the uploads directory exists
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads",userId);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generate unique filename and save
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Save relative path to DB
            track.FilePath = "/uploads/"+userId+"/" + fileName;
            track.CreatedAt = DateTime.UtcNow;
            track.UserId = userId;

            _context.Add(track);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Tracks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", track.UserId);
            return View(track);
        }

        // POST: Tracks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Track track, IFormFile? file)
        {
            if (id != track.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(track);
            }

            try
            {
                var existingTrack = await _context.Tracks.FindAsync(id);
                if (existingTrack == null)
                {
                    return NotFound();
                }

                // Update track details
                existingTrack.Title = track.Title;
                existingTrack.Genre = track.Genre;
                existingTrack.ReleaseDate = track.ReleaseDate;

                // Handle file upload if a new file is provided
                if (file != null && file.Length > 0)
                {
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads",track.UserId);

                    // Ensure uploads directory exists
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Generate unique filename
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Delete old file if it exists
                    if (!string.IsNullOrEmpty(existingTrack.FilePath))
                    {
                        var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingTrack.FilePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Update file path
                    existingTrack.FilePath = "/uploads/" + track.UserId + "/" + fileName;
                }

                _context.Update(existingTrack);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackExists(track.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        // GET: Tracks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // POST: Tracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track != null)
            {
                _context.Tracks.Remove(track);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackExists(int id)
        {
            return _context.Tracks.Any(e => e.Id == id);
        }
    }
}
