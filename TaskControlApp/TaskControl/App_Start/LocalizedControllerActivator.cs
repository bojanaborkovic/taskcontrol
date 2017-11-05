using BussinesService.Interfaces.Responses.User;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TaskControl.ServiceClients;


namespace TaskControl.App_Start
{
  public class LocalizedControllerActivator : IControllerActivator
  {
    private string _DefaultLanguage = "en";
    private string _dbLanguage;
    private UserServiceClient serviceClient = new UserServiceClient("users") { DoSerialize = true };

    public IController Create(RequestContext requestContext, Type controllerType)
    {
      //Get the {language} parameter in the RouteData
      string lang = string.Empty;
      string user = System.Web.HttpContext.Current.User.Identity.GetUserId();
      UserLanguageReturn dbLanguage = new UserLanguageReturn();

      //only available when player is logged in
      // check if the call has already been made and language info is stored in _dbLanguage var
      // if yes, skip another check (unnecessary call)
      if (string.IsNullOrEmpty(_dbLanguage))
      {
        if (!string.IsNullOrEmpty(user))
        {
          dbLanguage = serviceClient.GetUserLanguage(long.Parse(user));
          if (dbLanguage != null && dbLanguage.UserId != 0)
          {
            _dbLanguage = dbLanguage.LanguageCode;
          }
        }
      }
      else
      {
        if (user == null)
        {
          lang = "en";
        }
        else
        {
          var configuredLanguage = serviceClient.GetUserLanguage(long.Parse(user));
          if (configuredLanguage != null && string.IsNullOrEmpty(configuredLanguage.ErrorMessage))
          {
            if (_dbLanguage != configuredLanguage.LanguageCode)
            {
              _dbLanguage = configuredLanguage.LanguageCode;
            }
          }
        }
      }

    
      //language parameter does not exist in the route
      if (requestContext.RouteData.Values["lang"] == null)
      {
        //if exists set the db configured lang
        if (!string.IsNullOrEmpty(_dbLanguage))
        {
          lang = _dbLanguage;
        }
        
        //if not fallback to default
        else
        {
          lang = _DefaultLanguage;
        }
        //put it in the route      
        requestContext.RouteData.Values.Add("lang", lang);
      }
      //language parameter exists in the route
      else
      {
        //if exists set the db configured lang
        if (!string.IsNullOrEmpty(_dbLanguage))
        {
          lang = _dbLanguage;
          //if they are different, use the one configured in the db
          if(_dbLanguage != requestContext.RouteData.Values["lang"].ToString())
          {
            requestContext.RouteData.Values.Remove("lang");
            requestContext.RouteData.Values.Add("lang", lang);
          }
         
        }
        //if not set the one from the route
        else
        {
          lang = requestContext.RouteData.Values["lang"].ToString();
        }


      }

      if (lang != _DefaultLanguage)
      {
        try
        {
          Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
        }
        catch (Exception e)
        {
          throw new NotSupportedException(String.Format("ERROR: Invalid language code '{0}'. Error: {1}", lang, e.Message));
        }
      }

      return DependencyResolver.Current.GetService(controllerType) as IController;
    }
  }
}