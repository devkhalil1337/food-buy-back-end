﻿using System.Web;
using System.Web.Mvc;

namespace Food_buy_back_end
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
