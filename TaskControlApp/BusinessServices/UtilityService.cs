using AutoMapper;
using BusinessServices.Interfaces;
using DataModel;
using DataModel.UnitOfWork;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BusinessServices
{
  public class UtilityService : IUtilityService
  {
    private readonly UnitOfWork _unitOfWork = new UnitOfWork();
    internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof(UtilityService));

    public UtilityService()
    {
      _unitOfWork = new UnitOfWork();
    }

    public IEnumerable<IssueTypeEntity> GetIssueTypes()
    {
      _log.DebugFormat("GetIssueTypes invoked");
      try
      {
        var issueTypes = _unitOfWork.IssueTypeRepositorsy.GetAll();
        if (issueTypes.Any())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<IssueType, IssueTypeEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var usersMapped = mapper.Map<List<IssueType>, List<IssueTypeEntity>>(issueTypes.ToList());
          _log.DebugFormat("GetIssueTypes finished with : {0}", issueTypes.ToString());
          return usersMapped;
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching issue types... {0}", ex.Message);
      }


      return null;
    }

    public IEnumerable<StatusEntity> GetAllStatuses()
    {
      _log.DebugFormat("GetAllStatuses invoked");
      try
      {
        var statuses = _unitOfWork.StatusRepository.GetAll();
        if (statuses.Any())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<Status, StatusEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var statusMapped = mapper.Map<List<Status>, List<StatusEntity>>(statuses.ToList());
          _log.DebugFormat("GetAllStatuses finished with : {0}", statuses.ToString());
          return statusMapped;
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching statuses... {0}", ex.Message);
      }


      return null;
    }

    public IEnumerable<PriorityEntity> GetPriorities()
    {
      _log.DebugFormat("GetPriorities invoked");
      try
      {
        var prior = _unitOfWork.PriorityRepository.GetAll();
        if (prior.Any())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<Priority, PriorityEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var priorMapped = mapper.Map<List<Priority>, List<PriorityEntity>>(prior.ToList());
          _log.DebugFormat("GetPriorities finished with : {0}", prior.ToString());
          return priorMapped;
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching priorities... {0}", ex.Message);
      }

      return null;
    }
  }
}
