using AutoMapper;
using Localizard._context;
using Localizard.Domain.Entites;
using Localizard.Domain.Enums;
using Localizard.Domain.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Localizard.DAL.Repositories.Implementations;

public class LanguageRepo : ILanguageRepo
{
    
    private readonly ApplicationDbContext _context;

    public LanguageRepo(IMapper mapper, ApplicationDbContext context)
    {
        _context = context;
    }
    public List<Language> GetAll()
    {
        return _context.Languages.OrderBy(l => l.Id).ToList();
    }

    public async Task<Language> GetById(int id)
    {
        return await _context.Languages.FirstOrDefaultAsync(l => l.Id == id);
    }

    public bool LanguageExists(int id)
    {
        return _context.Languages.Any(l => l.Id == id);
    }

    public bool CteateLanguage(Language language)
    {
        _context.Add(language);
        return Save();
    }

    public bool UpdateLanguage(Language language)
    {
        _context.Update(language);
        return Save();
    }
    
    public bool Delete(User user)
    {
        throw new NotImplementedException();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}