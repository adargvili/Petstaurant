using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Petstaurant.Data;
using Petstaurant.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tweetinvi;

namespace Petstaurant.Controllers
{
    public class UsersController : Controller
    {
        private readonly PetstaurantContext _context;

        public UsersController(PetstaurantContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (user == null)
            {
                return NotFound();
            }
            var u = GetCurrentUserName();
            var t = GetCurrentUserType();
            if ((u != user.UserName) && t != "Admin")
            {
                return NotFound();
            }

            if (t == "Admin")
            {
                ViewData["UserAdmin"] = UserType.Admin;
            }

            return View(user);
        }

        // GET: Users/Register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) {
                return View("AlreadyLoggedIn");
            }
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserName,Password,ConfirmPassword,Gender,Name,BirthDate")] User user)
        {
            if (ModelState.IsValid)
            {

                if (user.BirthDate.Year > DateTime.Now.Year - 15 ||
                    user.BirthDate.Year < DateTime.Now.Year - 120 ||
                    !(user.UserName.EndsWith(".com") ||
                    user.UserName.EndsWith(".co.il") ||
                    user.UserName.EndsWith(".jp"))) { return NotFound(); }

                var q = _context.User.FirstOrDefault(u => u.UserName == user.UserName);

                if (q == null)
                {
                    user.Cart = new Cart();
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    var u = _context.User.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

                    Signin(u);

                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Unable to comply; cannot register this user.";
                }
            }
            return View(user);
        }
        // GET: Users/Login
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("AlreadyLoggedIn");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,Password")] User user)
        {
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
               
                var q = from u in _context.User
                        where u.UserName == user.UserName && u.Password == user.Password
                        select u;

                if (q.Count() > 0)
                {

                    Signin(q.First());

                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Username and/or password are incorrect.";
                }
            }
            return View(user);
        }

        private async void Signin(User account)
        {
            var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Email, account.UserName),
                new Claim(ClaimTypes.Role, account.UserType.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        // GET: Users/Edit/X
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var u = GetCurrentUserName();
            var t = GetCurrentUserType();
            if ((u != user.UserName) && t != "Admin")
            {
                return NotFound();
            }

            if (t == "Admin") {
                ViewData["UserAdmin"] = UserType.Admin;
            }
            ViewData["Password"] = user.Password;
            ViewData["ConfirmPassword"] = user.ConfirmPassword;
            return View(user);
        }

        // POST: Users/Edit/X
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,Password,ConfirmPassword,Gender,Name,BirthDate,UserType")] User user)
        {
            
            if (id != user.UserName)
            {
                return NotFound();
            }

            bool flag = false;
            if (GetCurrentUserType() == "Admin")
            {
                flag = true;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (flag&&user.UserType == UserType.Customer) {
                    return RedirectToAction(nameof(Logout));
                }
                return RedirectToAction(nameof(Details), new { @id=user.UserName});
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserName == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            var cartToDelete = _context.Cart.FirstOrDefault(c => c.UserName == user.UserName);

            _context.Cart.Remove(cartToDelete);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.UserName == id);
        }


        private string GetCurrentUserName()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var u = claims.First().Value;
            return u;
        }
    
        private string GetCurrentUserType()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var u = claims.Skip(1).First().Value;
            return u;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search(string queryUserName,string queryName, string selectType)
        {
            var userTypeAdmin = UserType.Admin.ToString();
            var userTypeCustomer = UserType.Customer.ToString();
            UserType choice;
            if (userTypeAdmin.Equals(selectType))
            {
                choice = UserType.Admin;
            }
            else
            {
                choice = UserType.Customer;
            }

            var q = from a in _context.User
                    where ((a.UserType == choice || selectType==null) && (a.UserName.Contains(queryUserName) || queryUserName==null) &&
                            (a.Name.Contains(queryName) || queryName == null))
                    orderby a.Name ascending
                    select a;
            
            return PartialView("Search", await q.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Tweet(string tweetData)
        {
            var petStaurantUser = new TwitterClient("OAHEhNknOQlBIwdEOIdaDMuZp", "Kz4nVWGWL1XHhwCD7DSDt6kmtPBuCiPPSMHJCI4aVcfuhwOb93", "865798995041505281-4j9YnKsVx8CPiJc77s2iBRU8bbVVEb3", "Ap5b8HluV6slMDTnKAF4JVkY6xcBjEfTH7RqpQV0fvxqC");
            await petStaurantUser.Tweets.PublishTweetAsync(tweetData);
            var u = GetCurrentUserName();
            return RedirectToAction(nameof(Index),"Home");
        }
    }
}
