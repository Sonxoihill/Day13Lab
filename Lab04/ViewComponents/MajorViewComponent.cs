using Microsoft.AspNetCore.Mvc;
using Lab04.Models;
using Lab04.Data;
namespace Lab04.ViewComponents
{
    public class MajorViewComponent:ViewComponent
    {
        SchoolDbContext db;
        List<Major> majors;
        public MajorViewComponent(SchoolDbContext _context)
        {
            db = _context;
            majors = db.Major.ToList();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RenderMajor", majors);
        }
    }
}
