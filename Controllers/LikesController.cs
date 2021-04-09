using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class LikesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public LikesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Likes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Likes.Include(l => l.Article).Include(l => l.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Likes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Likes
                .Include(l => l.Article)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LikeId == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // GET: Likes/Create
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            Like like = new Like();
            var UserId = _userManager.GetUserId(HttpContext.User);
            like.UserId = UserId;
            like.ArticleId = (int)id;
            like.CreatedAt = DateTime.Now;
            like.UpdatedAt = DateTime.Now;
            var article = (int)id;
            _context.Add(like);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Articles", new { id = article } );
        }

        // POST: Likes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ArticleId")] Like like)
        {
            if (ModelState.IsValid)
            {
                var UserId = _userManager.GetUserId(HttpContext.User);
                like.UserId = UserId;
                like.CreatedAt = DateTime.Now;
                like.UpdatedAt = DateTime.Now;

                _context.Add(like);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Articles");
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "ArticleId", "ArticleId", like.ArticleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", like.UserId);
            return RedirectToAction("Index", "Articles");
        }

       

        // GET: Likes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var like = await _context.Likes.FindAsync(id);
            var article = Convert.ToString(like.ArticleId);
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Articles", new { id = article });
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            var article = like.ArticleId;
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return RedirectToAction("/Articles/Details/"+article);
        }

        private bool LikeExists(int id)
        {
            return _context.Likes.Any(e => e.LikeId == id);
        }
    }
}
