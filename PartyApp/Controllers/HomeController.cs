using Microsoft.AspNetCore.Mvc;
using PartyInvitationManager.Models;
using System.Diagnostics;
using System;
using Microsoft.AspNetCore.Http;
using PartyInvitationManager.Models;

namespace PartyInvitationManager.Controllers
{
    public class HomeController : Controller
    {
        private const string FirstVisitCookieName = "FirstVisit";

        public IActionResult Index()
        {
            // Check for first visit cookie and set if not exist
            if (!Request.Cookies.ContainsKey(FirstVisitCookieName))
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1),
                    HttpOnly = true,
                    IsEssential = true
                };

                Response.Cookies.Append(FirstVisitCookieName, DateTime.Now.ToString("o"), cookieOptions);

                // For the first visit, we don't need to set ViewBag.FirstVisit since the view will check for null
            }
            else
            {
                // For returning visitors, parse the date from cookie
                if (DateTime.TryParse(Request.Cookies[FirstVisitCookieName], out DateTime firstVisit))
                {
                    ViewBag.FirstVisit = firstVisit;
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}