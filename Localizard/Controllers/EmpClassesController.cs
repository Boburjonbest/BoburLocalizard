using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Localizard._context;
using Localizard.Models;
using Microsoft.AspNetCore.Authorization;

namespace Localizard.Controllers
{
   
    public class EmpClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("api/[controller]")]
        
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.EmpClasses.ToListAsync());
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empClass = await _context.EmpClasses
                .FirstOrDefaultAsync(m => m.Name == id);
            if (empClass == null)
            {
                return NotFound();
            }

            return Ok(empClass);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return Ok();
        }

        [Authorize]
        [HttpPost("/Create")]
        public async Task<IActionResult> Create([Bind("Name,DefAultLanguage,AvailableLanguage")] EmpClass empClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Ok(empClass);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empClass = await _context.EmpClasses.FindAsync(id);
            if (empClass == null)
            {
                return NotFound();
            }
            return Ok(empClass);
        }

        public async Task<IActionResult> Edit(string id, [Bind("Name,DefaultLanguage,AvailableLanguage")] EmpClass empClass)
        {
            if (id != empClass.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpClassExists(empClass.Name))
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
            return Ok(empClass);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empClass = await _context.EmpClasses
                .FirstOrDefaultAsync(m => m.Name == id);
            if (empClass == null)
            {
                return NotFound();
            }

            return Ok(empClass);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var empClass = await _context.EmpClasses.FindAsync(id);
            if (empClass != null)
            {
                _context.EmpClasses.Remove(empClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpClassExists(string id)
        {
            return _context.EmpClasses.Any(e => e.Name == id);
        }

      
        public async Task<IActionResult> Create( Project myEntity)
        {
            if (ModelState.IsValid)
            {
                myEntity.CreatedAt = DateTime.Now;
                myEntity.UpdatedAt = DateTime.Now;
                _context.Add(myEntity);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return Ok(myEntity);
            
        }


    }
}
