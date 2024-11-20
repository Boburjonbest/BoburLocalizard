using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;
using Localizard.Domain.Entites;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Localizard.Domain.ViewModel;

public class ProjectDetailView
{
    public string Key { get; set; }
    public int TranslationId { get; set; }
    public string Description { get; set; }
    public string Tag { get; set; }
}