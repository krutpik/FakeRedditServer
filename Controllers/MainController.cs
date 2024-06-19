using System.Collections;
using System.ComponentModel.DataAnnotations;
using FakeReddit.Data;
using FakeReddit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace FakeReddit.Controllers
{
    public class MainController : Controller
    {
        private readonly DataBaseContext _context;
        
        public MainController(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ViewResult> Index()
        {
            var theme = await _context.Theme.OrderBy(m => m.Id).ToListAsync();
            return View(theme);
        }

        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task ChangeRate(Dictionary<string, string> data)
        {
            var theme = await  _context.Theme.FirstOrDefaultAsync(model => model.Id == Convert.ToUInt32(data["Id"]));
            if (theme == null) return;
            
            if (data["Rate"] == "+")
            {
                theme.Rate += 1;
            }
            else
            {
                theme.Rate -= 1;
            }
           
            _context.Theme.Update(theme);
            await _context.SaveChangesAsync();
        }
        
        [HttpPost]
        [ActionName("Create")]
        public async Task<ActionResult> CreateApi([Bind("Title")] Theme data)
        {
            if (ModelState.IsValid)
            {
                data.Date = DateTime.Now.ToUniversalTime(); 
                _context.Add(data);
                await _context.SaveChangesAsync();
                return Redirect(nameof(Index));
            }
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var theme = await _context.Theme.FirstOrDefaultAsync(m => m.Id == id);
            
            if (theme != null) _context.Theme.Remove(theme);
            await _context.SaveChangesAsync();
            
            return Redirect(nameof(Index));
        }
        
    }
}
