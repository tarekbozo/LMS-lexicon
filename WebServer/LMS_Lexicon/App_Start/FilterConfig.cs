﻿using System.Web;
using System.Web.Mvc;

namespace LMS_Lexicon
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
