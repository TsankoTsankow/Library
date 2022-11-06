using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("All", "Books");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFileCollection files)
        {
            var filePath = Path.GetTempFileName();

            foreach (var  file in files.Where(f => f.Length > 0))
            {
                var name = file.FileName;

                //За да записваме в базата - като например малки снимки
                using (MemoryStream ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    byte[] data = ms.ToArray(); 
                }

                //За да я запишем във файл някъде - например локално на машината
                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await file.CopyToAsync(stream);
                //}
            }

            return Ok(new
            {
                fileCount = files.Count,
                fileSize = files.Sum(f => f.Length)
            });
        }

        
    }
}