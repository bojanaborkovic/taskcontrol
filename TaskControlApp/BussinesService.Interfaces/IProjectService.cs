﻿using BussinesService.Interfaces.Responses.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BusinessServices.Interfaces
{
  public interface IProjectService
  {
    BaseProjectReturn GetProjectById(long Id);
    GetProjectReturn GetAllProjects();
    BaseProjectReturn CreateProject(ProjectEntity project);
    BaseProjectReturn UpdateProject(ProjectEntity project);
    GetProjectReturn GetProjectsWithOwner();

  }
}
