﻿using AspNetIdentity.WebApi.Infrastructure;
using AspNetIdentity.WebApi.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NLog;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Http.ExceptionHandling;

namespace AspNetIdentity.WebApi.Controllers
{
    public class BaseApiController : ApiController
    {
        protected readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;
        //
        private JVContext DBContext = null;
        public UserExtensionManager UserExtManager = null;
        public LookupDataManager LookupManager = null;
        public MarketManager MarketManager = null;
        public UserRatingManager RatingManager = null;

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        public BaseApiController()
        {
            DBContext = new JVContext();
            UserExtManager = new UserExtensionManager(DBContext);
            RatingManager = new UserRatingManager(DBContext);
            LookupManager = new LookupDataManager(DBContext);
            MarketManager = new MarketManager(DBContext, RatingManager);
            Configuration = new HttpConfiguration();
            Configuration.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                int i = 0;
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError(i++.ToString(), error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        protected void LogInfo(string msg, object obj)
        {
            logger.Info(msg + " {0}", new object[] { JsonConvert.SerializeObject(obj) });
        }
    }
}