﻿using Localizard._context;
using Localizard.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Localizard.DAL.Repositories.Implementations;

public class TranslationRepo : ITranslationRepo
{

    private readonly ApplicationDbContext _context;
    public TranslationRepo(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public ICollection<Translation> GetAll()
    {
        return _context.Translations.OrderBy(p => p.Id).ToList();
    }

    public async Task<Translation> GetById(int id)
    {
        return await _context.Translations.FirstOrDefaultAsync(p => p.Id == id);
    }

    public bool TranslationExists(int id)
    {
        return _context.Translations.Any(p => p.Id == id);
    }

    public bool CreateTranslation(Translation translation)
    {
        _context.Add(translation);
        return Save();
    }

    public bool UpdateTranslation(Translation translation)
    {
        _context.Update(translation);
        return Save();
    }

    public bool DeleteTranslation(Translation translation)
    {
        _context.Remove(translation);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}