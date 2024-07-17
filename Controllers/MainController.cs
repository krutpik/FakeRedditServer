using FakeReddit.Areas.Identity.Data;
using FakeReddit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FakeReddit.Controllers
{
    public class MainController : Controller
    {
        private readonly FakeRedditIdentityDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public MainController(FakeRedditIdentityDbContext context, UserManager<ApplicationUser>? userManager)
        {
            _context = context;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task<ViewResult> Index()
        {
            List<ThemeView> viewModel = [];

            var theme = await _context.Theme
                .OrderBy(m => m.Id)
                .Include(theme => theme.User)
                .ToListAsync();
            
            viewModel.AddRange(theme.Select(i => new ThemeView()
            {
                Id = i.Id,
                Title = i.Title,
                Rate = i.Rate,
                Date = i.Date,
                Writer = i.User.UserName
            }));
            return View(viewModel);
        }

        public async Task<ActionResult> Theme(int id)
        {
           var theme = await _context.Theme
               .Where(m => m.Id == id)
               .Include(theme => theme.User)
               .FirstOrDefaultAsync();

           if (theme is null) return NotFound();
           
           ThemeView themeView = new ThemeView()
           {
               Id = theme.Id,
               Title = theme.Title,
               Content = theme.Content,
               Date = theme.Date,
               Writer = theme.User.UserName,
           };
           
           return View(themeView);
        }
        
        [Authorize]
        public ViewResult Create()
        {
            return View();
        }

        /*
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task ChangeRate(Dictionary<string, string> data)
        {
            Theme? theme = await  _context.Theme.FirstOrDefaultAsync(model => model.Id == Convert.ToUInt32(data["Id"]));
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
        }*/
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<ActionResult> CreateApi(string title, string content)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }  
            
            var theme = new Theme()
            {
                Title = title,
                Content = content,
                
            };
            
            ApplicationUser user = (await _userManager.GetUserAsync(User)) ?? throw new InvalidOperationException();
            await _context.Entry(user).Collection(m => m.Themes).LoadAsync();
            user.Themes.Add(theme);
            await _context.SaveChangesAsync();
            
            return Redirect(nameof(Index)); 
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            Theme theme = await _context.Theme.FirstOrDefaultAsync(m => m.Id == id) ?? throw new InvalidCastException();
            
            await _context.Entry(theme).Reference(m => m.User).LoadAsync();
            
            if (_userManager.GetUserAsync(User).Result == theme.User || User.IsInRole("Admin"))
            {
                _context.Theme.Remove(theme);
                await _context.SaveChangesAsync();
            }
            else
            {
                return View("Error");
            }
            
            return Redirect(nameof(Index));
        }
        
    }
}
