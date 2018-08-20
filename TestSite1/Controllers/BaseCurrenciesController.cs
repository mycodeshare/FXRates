using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TestSite1.Models;

namespace TestSite1.Controllers
{
    public class BaseCurrenciesController : Controller
    {
        private readonly TestSite1Context _context;

        public BaseCurrenciesController(TestSite1Context context)
        {
            _context = context;
        }

        // GET: BaseCurrencies
        public async Task<IActionResult> Index()
        {
            return View(await _context.BaseCurrency.Include(BaseCurrency=>BaseCurrency.rates).ToListAsync());
        }

        // GET: BaseCurrencies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseCurrency = await _context.BaseCurrency
                .SingleOrDefaultAsync(m => m.id == id);
            if (baseCurrency == null)
            {
                return NotFound();
            }

            return View(baseCurrency);
        }

        // GET: BaseCurrencies/Create
        public async Task<IActionResult> Create()
        {
            //Initialize HTTP Client for calling the service
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://openexchangerates.org/api/latest.json?app_id=65368373a86d467481480393f8180482&base=usd");
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                
                ServiceData.RootObject responseElement = new ServiceData.RootObject();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                String responseString = await response.Content.ReadAsStringAsync(); //Call Service
                responseElement = JsonConvert.DeserializeObject<ServiceData.RootObject>(responseString, settings);  //Deserialize JSON to C# Object

                // Convert Service Data Transfer Object (DTO) to Database Object to persist in database
                BaseCurrency newItem = new BaseCurrency();
                newItem.baseData = responseElement.@base;
                newItem.disclaimer = responseElement.disclaimer;
                newItem.license = responseElement.license;
                newItem.timestamp = responseElement.timestamp;

                Rates newRate = new Rates();
                newRate.AED = responseElement.rates.AED;
                newRate.AFN = responseElement.rates.AFN;
                newRate.ALL = responseElement.rates.ALL;
                newRate.AMD = responseElement.rates.AMD;
                newRate.ANG = responseElement.rates.ANG;
                newRate.CAD = responseElement.rates.CAD;

                newItem.rates = new List<Rates>();
                newItem.rates.Add(newRate);

                //Persist DTO in Database
                _context.Add(newItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(await _context.BaseCurrency.ToListAsync());
        }

        // POST: BaseCurrencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,disclaimer,license,timestamp,baseData")] BaseCurrency baseCurrency)
        {
            if (ModelState.IsValid)
            {
                _context.Add(baseCurrency);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(baseCurrency);
        }

        // GET: BaseCurrencies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseCurrency = await _context.BaseCurrency.SingleOrDefaultAsync(m => m.id == id);
            if (baseCurrency == null)
            {
                return NotFound();
            }
            return View(baseCurrency);
        }

        // POST: BaseCurrencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,disclaimer,license,timestamp,baseData")] BaseCurrency baseCurrency)
        {
            if (id != baseCurrency.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baseCurrency);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaseCurrencyExists(baseCurrency.id))
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
            return View(baseCurrency);
        }

        // GET: BaseCurrencies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseCurrency = await _context.BaseCurrency
                .SingleOrDefaultAsync(m => m.id == id);
            if (baseCurrency == null)
            {
                return NotFound();
            }

            return View(baseCurrency);
        }

        // POST: BaseCurrencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baseCurrency = await _context.BaseCurrency.SingleOrDefaultAsync(m => m.id == id);
            _context.BaseCurrency.Remove(baseCurrency);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaseCurrencyExists(int id)
        {
            return _context.BaseCurrency.Any(e => e.id == id);
        }
    }
}
