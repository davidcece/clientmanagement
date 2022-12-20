using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClientManagement.Models;
using System.Data;

namespace ClientManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Cellphone,EmailStatus,SmsStatus")] Client client)
        {
            ValidateCell(client);

            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                UpdateSmsStatus(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Cellphone,EmailStatus,SmsStatus")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            ValidateCell(client);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                    UpdateSmsStatus(client);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }


        private void ValidateCell(Client client)
        {
            if (ModelState.IsValid)
            {
                var old = _context.Clients.FirstOrDefault(c => c.Email==client.Email);
                if(old != null)
                {
                    //Dont track this entry
                    _context.Entry(old).State = EntityState.Detached;
                }
                if (old!=null && old.Id!=client.Id)
                {
                    ModelState.AddModelError("Email", "Email must be unique");
                    return;
                }
                

                int len = client.Cellphone.Length;
                string prefix;
                switch (len)
                {
                    case 10:
                        prefix = client.Cellphone[..2];
                        if (prefix!="05")
                        {
                            ModelState.AddModelError("Cellphone", "Local phone must begin with 05");
                        }
                        break;
                    case 12:
                        prefix = client.Cellphone[..4];
                        if (prefix!="9725")
                        {
                            ModelState.AddModelError("Cellphone", "International phone must begin with 9725");
                        }
                        break;
                    default:
                        ModelState.AddModelError("Cellphone", "Cellphone length must be 10 for local or 12 for international number");
                        break;
                }

                if (len>=9)
                {
                    client.InternationalPhone=string.Concat("972", client.Cellphone.AsSpan(len-9, 9));
                }
            }
        }


        private void UpdateSmsStatus(Client client)
        {
            var clients = _context.Clients.Where(c => c.InternationalPhone==client.InternationalPhone).ToList();
            clients.ForEach(c => c.SmsStatus=client.SmsStatus);
            _context.SaveChanges();
        }


    }
}
