﻿using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AnyChat.Models;

namespace AnyChat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer(new AnyChatInitializer());
        }

        protected void Application_EndRequest()
        {
            //here breakpoint
            // under debug mode you can find the exceptions at code: this.Context.AllErrors
            var allErros = this.Context.AllErrors;
        }
        /// <summary>
        /// Session_Startイベントハンドラを明示的に宣言することによって
        /// Sessionに値を代入しなくてもSessionIDを使用できるようにする為宣言する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
        }
    }
}
