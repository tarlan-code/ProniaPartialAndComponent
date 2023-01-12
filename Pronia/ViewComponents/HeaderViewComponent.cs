using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;

namespace Pronia.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        readonly AppDbContext _context;

        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            var settings = _context.Settings.ToDictionary(s => s.Key, s => s.Value);
            return Task.FromResult<IViewComponentResult>(View(settings));
        }
    }
}
