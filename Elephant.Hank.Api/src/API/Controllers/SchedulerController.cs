﻿// ---------------------------------------------------------------------------------------------------
// <copyright file="SchedulerController.cs" company="Elephant Insurance Services, LLC">
//     Copyright (c) 2015 All Right Reserved
// </copyright>
// <author>Gurpreet Singh</author>
// <date>2015-04-20</date>
// <summary>
//     The SchedulerController class
// </summary>
// ---------------------------------------------------------------------------------------------------

namespace Elephant.Hank.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Elephant.Hank.Api.Security;
    using Elephant.Hank.Common.LogService;
    using Elephant.Hank.Common.TestDataServices;
    using Elephant.Hank.Framework.Extensions;
    using Elephant.Hank.Resources.Dto;
    using Elephant.Hank.Resources.Dto.Linking;
    using Elephant.Hank.Resources.Enum;
    using Elephant.Hank.Resources.Messages;

    /// <summary>
    /// The LocatorController class
    /// </summary>
    [RoutePrefix("api/website/{websiteId}/scheduler")]
    public class SchedulerController : BaseApiController
    {
        /// <summary>
        /// The locator service
        /// </summary>
        private readonly ISchedulerService schedulerService;

        /// <summary>
        /// The scheduler suite service
        /// </summary>
        private readonly ISchedulerSuiteMapService schedulerSuiteMapService;

        /// <summary>
        /// The scheduler history service
        /// </summary>
        private readonly ISchedulerHistoryService schedulerHistoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerController" /> class.
        /// </summary>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="schedulerService">The scheduler service.</param>
        /// <param name="schedulerSuiteMapService">The schedulerSuite service.</param>
        /// <param name="schedulerHistoryService">The scheduler history service.</param>
        public SchedulerController(
            ILoggerService loggerService,
            ISchedulerService schedulerService,
            ISchedulerSuiteMapService schedulerSuiteMapService, 
            ISchedulerHistoryService schedulerHistoryService)
            : base(loggerService)
        {
            this.schedulerService = schedulerService;
            this.schedulerSuiteMapService = schedulerSuiteMapService;
            this.schedulerHistoryService = schedulerHistoryService;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="websiteId">The website identifier.</param>
        /// <returns>List of TblSchedulerDto objects</returns>
        [Route]
        [CustomAuthorize(ActionType = ActionTypes.Read, ModuleType = FrameworkModules.Scheduler)]
        public IHttpActionResult GetAll(long websiteId)
        {
            var result = new ResultMessage<IEnumerable<TblSchedulerDto>>();
            try
            {
                result = this.schedulerService.GetByWebsiteId(websiteId);
            }
            catch (Exception ex)
            {
                this.LoggerService.LogException(ex);
                result.Messages.Add(new Message(null, ex.Message));
            }

            return this.CreateCustomResponse(result);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="schedulerId">The identifier.</param>
        /// <returns>TblSchedulerDto objects</returns>
        [Route("{schedulerId}")]
        [CustomAuthorize(ActionType = ActionTypes.Read, ModuleType = FrameworkModules.Scheduler)]
        public IHttpActionResult GetById(long schedulerId)
        {
            var result = new ResultMessage<TblSchedulerDto>();
            try
            {
                result = this.schedulerService.GetById(schedulerId);
            }
            catch (Exception ex)
            {
                this.LoggerService.LogException(ex);
                result.Messages.Add(new Message(null, ex.Message));
            }

            return this.CreateCustomResponse(result);
        }

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <param name="schedulerId">The identifier.</param>
        /// <returns>Deleted object</returns>
        [Route("{schedulerId}")]
        [HttpDelete]
        [CustomAuthorize(ActionType = ActionTypes.Delete, ModuleType = FrameworkModules.Scheduler)]
        public IHttpActionResult DeleteById(long schedulerId)
        {
            var result = new ResultMessage<TblSchedulerDto>();
            try
            {
                result = this.schedulerService.DeleteById(schedulerId, this.UserId);
            }
            catch (Exception ex)
            {
                this.LoggerService.LogException(ex);
                result.Messages.Add(new Message(null, ex.Message));
            }

            return this.CreateCustomResponse(result);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="schedulerDto">The scheduler dto.</param>
        /// <returns>
        /// Newly added object
        /// </returns>
        [HttpPost]
        [Route("")]
        [CustomAuthorize(ActionType = ActionTypes.Write, ModuleType = FrameworkModules.Scheduler)]
        public IHttpActionResult Add([FromBody]TblSchedulerDto schedulerDto)
        {
            return this.AddUpdate(schedulerDto);
        }

        /// <summary>
        /// Updates the specified action dto.
        /// </summary>
        /// <param name="schedulerDto">The locator dto.</param>
        /// <param name="schedulerId">The identifier.</param>
        /// <returns>
        /// Newly updated object
        /// </returns>
        [Route("{schedulerId}")]
        [HttpPut]
        [CustomAuthorize(ActionType = ActionTypes.Write, ModuleType = FrameworkModules.Scheduler)]
        public IHttpActionResult Update([FromBody]TblSchedulerDto schedulerDto, long schedulerId)
        {
            schedulerDto.Id = schedulerId;
            schedulerDto.LastExecuted = null;
            return this.AddUpdate(schedulerDto);
        }

        #region Scheduler - Child History

        /// <summary>
        /// Get Scheduler Suites
        /// </summary>
        /// <param name="schedulerId">the scheduler identifier</param>
        /// <returns>TblSchedulerSuiteDto objects</returns>
        [Route("{schedulerId}/scheduler-history")]
        [CustomAuthorize(ActionType = ActionTypes.Read, ModuleType = FrameworkModules.Scheduler)]
        public IHttpActionResult GetSchedulerHistoryById(long schedulerId)
        {
            var result = new ResultMessage<IEnumerable<TblSchedulerHistoryDto>>();
            try
            {
                result = this.schedulerHistoryService.GetBySchedulerId(schedulerId);
            }
            catch (Exception ex)
            {
                this.LoggerService.LogException(ex);
                result.Messages.Add(new Message(null, ex.Message));
            }

            return this.CreateCustomResponse(result);
        }

        /// <summary>
        /// sets the force execute flag to true
        /// </summary>
        /// <param name="schedulerId">the scheduler identifier</param>
        /// <returns>TblSchedulerSuiteDto objects</returns>
        [HttpPost]
        [Route("{schedulerId}/force-execute")]
        [CustomAuthorize(ActionType = ActionTypes.Execute, ModuleType = FrameworkModules.Scheduler)]
        public IHttpActionResult ForceExecute(long schedulerId)
        {
            var result = new ResultMessage<TblSchedulerDto>();
            try
            {
                result = this.schedulerService.ForceExecute(this.UserId, schedulerId, null, null, null);
            }
            catch (Exception ex)
            {
                this.LoggerService.LogException(ex);
                result.Messages.Add(new Message(null, ex.Message));
            }

            return this.CreateCustomResponse(result);
        }

        /// <summary>
        /// Forces the execute now.
        /// </summary>
        /// <param name="schedulerId">The scheduler identifier.</param>
        /// <param name="target">The target.</param>
        /// <param name="port">The port.</param>
        /// <returns>Group name of batch</returns>
        [HttpGet]
        [Route("{schedulerId}/force-execute/{target}/{port?}")]
        [CustomAuthorize(ActionType = ActionTypes.Execute, ModuleType = FrameworkModules.Scheduler)]
        public IHttpActionResult ForceExecuteNow(long schedulerId, string target, int? port = null)
        {
            var result = new ResultMessage<string>();
            try
            {
                var resultDto = this.schedulerService.ForceExecute(this.UserId, schedulerId, target, port, null);

                result.Messages.AddRange(resultDto.Messages);

                if (resultDto.Item != null)
                {
                    result.Item = resultDto.Item.ExecutionGroupName;
                }
            }
            catch (Exception ex)
            {
                this.LoggerService.LogException(ex);
                result.Messages.Add(new Message(null, ex.Message));
            }

            return this.CreateCustomResponse(result);
        }

        /// <summary>
        /// sets the force execute flag to true by external entities
        /// </summary>
        /// <param name="schedulerId">the scheduler identifier</param>
        /// <returns>TblSchedulerSuiteDto objects</returns>
        [HttpPost]
        [Route("{schedulerId}/force-execute-external")]
        public async Task<IHttpActionResult> ForceExecuteExternal(long schedulerId)
        {
            var result = new ResultMessage<TblSchedulerDto>();
            try
            {
                string requestContent = await Request.Content.ReadAsStringAsync();

                result = this.schedulerService.ForceExecute(this.UserId, schedulerId, null, null, requestContent);
            }
            catch (Exception ex)
            {
                this.LoggerService.LogException(ex);
                result.Messages.Add(new Message(null, ex.Message));
            }

            return this.CreateCustomResponse(result);
        }

        #endregion

        #region Scheduler - Mapping

        /// <summary>
        /// Adds the multiple suite links.
        /// </summary>
        /// <param name="schedulerSuiteDto">The suite test dto.</param>
        /// <param name="schedulerId">The scheduler identifier.</param>
        /// <returns>
        /// TblLnkSuiteTestDto objects
        /// </returns>
        [Route("{schedulerId}/scheduler-suite-map")]
        [HttpPost]
        [CustomAuthorize(ActionType = ActionTypes.Write, ModuleType = FrameworkModules.Scheduler)]
        public IHttpActionResult AddMultipleSuiteLinks([FromBody]List<TblLnkSchedulerSuiteDto> schedulerSuiteDto, long schedulerId)
        {
            var result = new ResultMessage<IEnumerable<TblLnkSchedulerSuiteDto>>();
            try
            {
                result = this.schedulerSuiteMapService.SaveOrUpdate(this.UserId, schedulerId, schedulerSuiteDto);
            }
            catch (Exception ex)
            {
                this.LoggerService.LogException(ex);
                result.Messages.Add(new Message(null, ex.Message));
            }

            return this.CreateCustomResponse(result);
        }

        /// <summary>
        /// Get Scheduler Suites
        /// </summary>
        /// <param name="schedulerId">the scheduler identifier</param>
        /// <returns>TblSchedulerSuiteDto objects</returns>
        [Route("{schedulerId}/scheduler-suite-map")]
        [CustomAuthorize(ActionType = ActionTypes.Read, ModuleType = FrameworkModules.Scheduler)]
        public IHttpActionResult GetSchedulerSuite(long schedulerId)
        {
            var result = new ResultMessage<IEnumerable<TblLnkSchedulerSuiteDto>>();
            try
            {
                result = this.schedulerSuiteMapService.GetBySchedulerId(schedulerId);
            }
            catch (Exception ex)
            {
                this.LoggerService.LogException(ex);
                result.Messages.Add(new Message(null, ex.Message));
            }

            return this.CreateCustomResponse(result);
        }

        #endregion

        #region Scheduler - All Private

        /// <summary>
        /// Adds the update.
        /// </summary>
        /// <param name="schedulerDto">The scheduler dto.</param>
        /// <returns>
        /// Newly added object
        /// </returns>
        private IHttpActionResult AddUpdate(TblSchedulerDto schedulerDto)
        {
            var result = new ResultMessage<TblSchedulerDto>();
            try
            {
                result = this.schedulerService.SaveOrUpdate(schedulerDto, this.UserId);
            }
            catch (Exception ex)
            {
                this.LoggerService.LogException(ex);
                result.Messages.Add(new Message(null, ex.Message));
            }

            return this.CreateCustomResponse(result);
        }

        #endregion
    }
}