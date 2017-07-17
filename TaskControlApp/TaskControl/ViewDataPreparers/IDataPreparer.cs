using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace TaskControl.ViewDataPreparers
{
	interface IDataPreparer<TModel>
	{
		IList<TModel> GetDataItems();
	}
}