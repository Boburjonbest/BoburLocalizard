﻿using AutoMapper;
using System.Linq;
using System.Security.Claims;
using Localizard.DAL.Repositories;
using Localizard.Domain.Entites;
using Localizard.Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Localizard.Controller;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepo _projectRepo;
    private readonly IProjectDetailRepo _projectDetailRepo;
    private readonly ILanguageRepo _languageRepo;
    private readonly IMapper _mapper;
    
    public ProjectController(IProjectRepo projectRepo, IMapper mapper, IProjectDetailRepo projectDetailRepo, ILanguageRepo languageRepo)
    {
        _projectRepo = projectRepo;
        _mapper = mapper;
        _projectDetailRepo = projectDetailRepo;
        _languageRepo = languageRepo;
    }


    [HttpGet]
    public  async Task<IActionResult> GetAllProjects()
    {
        var projects = _projectRepo.GetAllProjects();
        
        var projectInfoViews = new List<GetProjectView>(); 
        foreach (var project in projects)
        {
            var projectinfoView = ProjectViewMapper(project);
            projectinfoView.DefaultLanguage = await _languageRepo.GetById(project.LanguageId);
            projectInfoViews.Add(projectinfoView);
        }
        return Ok(projectInfoViews);
    }
    
   
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        if (!_projectRepo.ProjectExists(id))
            return NotFound();

        var project = await _projectRepo.GetById(id);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(project);
    }
    
    
    private ProjectInfo ProjectInfoMapper(CreateProjectView createProjectCreate)
    {
        ProjectInfo projectInfo = new ProjectInfo()
        {
            Name = createProjectCreate.Name,
            LanguageId = createProjectCreate.DefaultLanguageId,
            CreatedAt = createProjectCreate.CreatedAt,
            UpdatedAt = createProjectCreate.UpdatedAt
        };
        
        projectInfo.UserId = int.Parse(User.FindFirst("id").Value);
        return projectInfo;
    }

    private GetProjectView ProjectViewMapper(ProjectInfo projectInfo)
    {
        GetProjectView createProjectView = new GetProjectView()
        {
            Name = projectInfo.Name,
            CreatedAt = projectInfo.CreatedAt,
            UpdatedAt = projectInfo.UpdatedAt,
            ProjectDetail = projectInfo.ProjectDetail,
            AvialableLanguages = projectInfo.Languages,
            
            
        };
        return createProjectView;
    }
    
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectView createProjectCreate)
    {
        if (createProjectCreate == null)
            return BadRequest(ModelState);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var projectExists = _projectRepo.GetAllProjects()
            .Any(x => x.Name == createProjectCreate.Name && x.UserId == userId);
        
        var projectInfo =  ProjectInfoMapper(createProjectCreate);
        
        projectInfo.UserId = userId;

        var projectDetail = await _projectDetailRepo.GetById(createProjectCreate.ProjectDetailId);
        if (projectDetail is not null)
        {
            projectInfo.ProjectDetail = projectDetail;
        }

        var languages =  _languageRepo.GetAll();

        foreach (var language in languages)
        {
            if (projectInfo.Languages is null)
                projectInfo.Languages = new List<Language>();
            
            projectInfo.Languages.Add(language);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_projectRepo.CreateProject(projectInfo))
        {
            ModelState.AddModelError("", "Something went wrong! while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{projectId}")]
    public IActionResult UpdateProject(int projectId, [FromBody] UpdateProjectView updatedProject)
    {
        if (updatedProject == null)
            return BadRequest(ModelState);
    
        if (projectId != updatedProject.Id)
            return BadRequest(ModelState);
    
        if (!_projectRepo.ProjectExists(projectId))
            return NotFound();
    
        if (!ModelState.IsValid)
            return BadRequest();
    
        var projectMap = _mapper.Map<ProjectInfo>(updatedProject);
    
        if (!_projectRepo.UpdateProject(projectMap))
        {
            ModelState.AddModelError("","Something went wrong while updating");
            return StatusCode(500, ModelState);
        }
    
        return NoContent();
    }
}