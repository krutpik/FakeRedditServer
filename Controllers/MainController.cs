using System.Security.Claims;
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


        public MainController(FakeRedditIdentityDbContext context, UserManager<ApplicationUser>? userManager )
        {
            _context = context;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task<ViewResult> Index()
        {
            List<ThemeView> viewListModel = [];

            var theme = await _context.Theme
                .OrderBy(m => m.Id)
                .Include(theme => theme.User)
                .ToListAsync();
            
            viewListModel.AddRange(theme.Select(i => new ThemeView()
            {
                Id = i.Id,
                Title = i.Title,
                Rate = i.Rate,
                Date = i.Date,
                Writer = i.User.UserName
            }));
            return View(viewListModel);
        }

        public async Task<ActionResult> Theme(int id)
        {
           var theme = await _context.Theme
               .Where(m => m.Id == id)
               .Include(theme => theme.User)
               .Include(theme => theme.Comments)
               .ThenInclude(comment => comment.User)
               .FirstOrDefaultAsync();

           if (theme is null) return NotFound();

           List<CommentView> comments = [];
           comments.AddRange(theme.Comments.OrderByDescending(m => m.Id).Select(i => new CommentView() 
               { 
                   id = i.Id, 
                   Content = i.Content, 
                   Writer = i.User.UserName, 
               }));

           ThemeView themeView = new ThemeView()
           {
               Id = theme.Id,
               Title = theme.Title,
               Content = theme.Content,
               Date = theme.Date,
               Writer = theme.User.UserName,
               Comments = comments,
           };
           
           return View(themeView);
        }
        
        [Authorize]
        public ViewResult Create()
        {
            return View();
        }

        [Authorize]
        public ViewResult Edit(ThemeView theme)
        {
            return View(theme);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteComment(int id, int themeId)
        {
            Comment comment = await _context.Comment.FirstOrDefaultAsync(m => m.Id == id) ?? throw new InvalidCastException();
            
            await _context.Entry(comment).Reference(m => m.User).LoadAsync();
            
            if (_userManager.GetUserAsync(User).Result == comment.User || User.IsInRole("Admin"))
            {
                _context.Comment.Remove(comment);
                await _context.SaveChangesAsync();
            }
            else
            {
                return View("Error");
            }
            
            return Redirect($"Theme/{themeId}");
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comment(int id, string content)
        {
            if (!ModelState.IsValid)
            {
                return Content("Недействительные данные");
            }

            Comment comment = new Comment()
            {
                Content = content,
                
            };
            ApplicationUser user = await _userManager.GetUserAsync(User) ?? throw new InvalidOperationException();
            await _context.Entry(user).Collection(m => m.Comments).LoadAsync();
            user.Comments.Add(comment);

            Theme theme = await _context.Theme.FindAsync(id) ?? throw new InvalidOperationException();
            await _context.Entry(theme).Collection(m => m.Comments).LoadAsync();
            theme.Comments.Add(comment);

            await _context.SaveChangesAsync();
            
            return Redirect($"Theme/{id}");
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<ActionResult> CreateApi(string title, string content)
        {
            if (!ModelState.IsValid)
            {
                return Content("Недействительные данные");
            }  
            
            var theme = new Theme()
            {
                Title = title,
                Content = content,
                
            };
            
            ApplicationUser user = await _userManager.GetUserAsync(User) ?? throw new InvalidOperationException();
            await _context.Entry(user).Collection(m => m.Themes).LoadAsync();
            user.Themes.Add(theme);
            await _context.SaveChangesAsync();
            
            return Redirect(nameof(Index)); 
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<ActionResult> Edit(EditThemeModel themeView)
        {
            if (!ModelState.IsValid)
            {
                return Content("Недействительные данные");
            }
            
            Theme theme = _context.Theme
                .Include(theme => theme.User)
                .First(m => m.Id == themeView.Id) ?? throw new InvalidOperationException();

            if (_userManager.GetUserAsync(User).Result == theme.User || User.IsInRole("Admin"))
            {
                theme.Content = themeView.Content;
                theme.Title = themeView.Title;

                _context.Theme.Update(theme);
                await _context.SaveChangesAsync();
            }
            else
            {
                return View("Error");
            }
            
            return Redirect($"Theme/{themeView.Id}");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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
