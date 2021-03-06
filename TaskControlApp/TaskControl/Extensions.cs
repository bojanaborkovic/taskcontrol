﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TaskControl
{
  public static class Extensions
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="sortOrder"> ascending or descending</param>
    /// <param name="sortField"> field we are sorting by</param>
    /// <returns></returns>
    public static IHtmlString SortIdentifier(this HtmlHelper htmlHelper, string sortOrder, string sortField)
    {
      if (string.IsNullOrEmpty(sortOrder) || (sortOrder.Trim() != sortField && sortOrder.Replace("_desc", "").Trim() != sortField)) return null;

      string glyph = "";
      if (sortOrder.ToLower().Contains("desc"))
      {
        glyph = "glyphicon glyphicon-chevron-down";
      }
      else
      {
        glyph = "glyphicon glyphicon-chevron-up";
      }

      var span = new TagBuilder("span");
      span.Attributes["class"] = glyph;

      return MvcHtmlString.Create(span.ToString());

    }

    /// <summary>
    /// Converts a NameValueCollection into a RouteValueDictionary containing all of the elements in the collection, and optionally appends
    /// {newKey: newValue} if they are not null
    /// </summary>
    /// <param name="collection">NameValue collection to convert into a RouteValueDictionary</param>
    /// <param name="newKey">the name of a key to add to the RouteValueDictionary</param>
    /// <param name="newValue">the value associated with newKey to add to the RouteValueDictionary</param>
    /// <returns>A RouteValueDictionary containing all of the keys in collection, as well as {newKey: newValue} if they are not null</returns>
    public static RouteValueDictionary ToRouteValueDictionary(this NameValueCollection collection, string newKey, string newValue)
    {
      var routeValueDictionary = new RouteValueDictionary();
      foreach (var key in collection.AllKeys)
      {
        if (key == null) continue;
        if (routeValueDictionary.ContainsKey(key))
          routeValueDictionary.Remove(key);

        routeValueDictionary.Add(key, collection[key]);
      }
      if (string.IsNullOrEmpty(newValue))
      {
        routeValueDictionary.Remove(newKey);
      }
      else
      {
        if (routeValueDictionary.ContainsKey(newKey))
          routeValueDictionary.Remove(newKey);

        routeValueDictionary.Add(newKey, newValue);
      }
      return routeValueDictionary;
    }
  }
}