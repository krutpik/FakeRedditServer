using FakeReddit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MvcMovie.Data;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FakeReddit.Controllers
{
    public class MainController : Controller
    {
        private readonly DataBaseContext _context;
        
        public MainController(DataBaseContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var lak = new Paper()
            {
                Title = "test",
                Content = "wkfjajkgjsd;kgjskfjg;kdsjosidj",
                    
            };
            _context.Add(lak);
            _context.SaveChanges();
            return View();
        }

    }
}
