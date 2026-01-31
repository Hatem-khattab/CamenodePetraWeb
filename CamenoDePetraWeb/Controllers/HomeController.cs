using CamenoDePetraWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace CamenoDePetraWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ChangeLanguage(string lang)
        {
            // اللغات المسموحة
            var supportedLanguages = new[] { "en", "fr", "es","pt" };

            // إذا اللغة غير موجودة أو غير مدعومة
            if (string.IsNullOrEmpty(lang) || !supportedLanguages.Contains(lang))
            {
                lang = "en"; // الافتراضي
            }

            // تغيير الثقافة
            var culture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // تخزين اللغة في Cookie
            Response.Cookies.Append(
                "Language",
                lang,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                }
            );

            // الرجوع لنفس الصفحة
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}
