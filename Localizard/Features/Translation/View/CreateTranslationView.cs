﻿using Localizard.Domain.Entites;

namespace Localizard.Domain.ViewModel;

public class CreateTranslationView
{
    public string Key { get; set; }
    public string Language { get; set; }
    public string Text { get; set; }
}