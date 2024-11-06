using Localizard._context;
using Localizard.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Localizard.Views.Services
{
    public class ProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        { _context = context; }

        public async Task<PagedResult<Project>> GetPagedEntitiesAsync(int pageNumber, int pageSize, string nameFilter = null)
        {
            var query = _context.MyEntities.AsQueryable();

            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(e => e.Name.Contains(nameFilter));
            }


            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);


            var data = await query
                .OrderBy(e => e.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Project>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                Data = data

                
            };

            
        }
        
    }
}
