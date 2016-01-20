﻿// ---------------------------------------------------------------------------------------------------
// <copyright file="TestQueueExecutableService.cs" company="Elephant Insurance Services, LLC">
//     Copyright (c) 2015 All Right Reserved
// </copyright>
// <author>Vyom Sharma</author>
// <date>2015-05-25</date>
// <summary>
//     The TestQueueExecutableService class
// </summary>
// ---------------------------------------------------------------------------------------------------

namespace Elephant.Hank.Framework.TestDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Elephant.Hank.Common.DataService;
    using Elephant.Hank.Common.Mapper;
    using Elephant.Hank.Common.TestDataServices;
    using Elephant.Hank.DataService.DBSchema;
    using Elephant.Hank.Resources.Dto;
    using Elephant.Hank.Resources.Enum;
    using Elephant.Hank.Resources.Extensions;
    using Elephant.Hank.Resources.Json;
    using Elephant.Hank.Resources.Messages;
    using Elephant.Hank.Resources.Models;
    using Elephant.Hank.Resources.ViewModal;

    /// <summary>
    /// The TestQueueExecutableService class
    /// </summary>
    public class TestQueueExecutableService : ITestQueueExecutableService
    {
        /// <summary>
        /// The suite service
        /// </summary>
        private readonly ISuiteService suiteService;

        /// <summary>
        /// The scheduler service
        /// </summary>
        private readonly ISchedulerService schedulerService;

        /// <summary>
        /// The test queue service
        /// </summary>
        private readonly ITestQueueService testQueueService;

        /// <summary>
        /// The test queue service
        /// </summary>
        private readonly IActionsService actionService;

        /// <summary>
        /// the browser service
        /// </summary>
        private readonly IBrowserService browserService;

        /// <summary>
        /// The mapper factory
        /// </summary>
        private readonly IMapperFactory mapperFactory;

        /// <summary>
        /// The table test data
        /// </summary>
        private readonly IRepository<TblTestData> table;

        /// <summary>
        /// The automatic gen array
        /// </summary>
        private List<AutoGenModel> autoGenArray;

        /// <summary>
        /// Gets or sets the testPlan
        /// </summary>
        private List<TblTestDataDto> testPlan = new List<TblTestDataDto>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TestQueueExecutableService" /> class.
        /// </summary>
        /// <param name="table">The table test data.</param>
        /// <param name="suiteService">The suite service.</param>
        /// <param name="testQueueService">The test queue service.</param>
        /// <param name="mapperFactory">The mapper factory.</param>
        /// <param name="schedulerService">The scheduler service.</param>
        /// <param name="browserService">The browser service.</param>
        /// <param name="actionService">The action service.</param>
        public TestQueueExecutableService(IRepository<TblTestData> table, ISuiteService suiteService, ITestQueueService testQueueService, IMapperFactory mapperFactory, ISchedulerService schedulerService, IBrowserService browserService, IActionsService actionService)
        {
            this.table = table;
            this.suiteService = suiteService;
            this.mapperFactory = mapperFactory;
            this.schedulerService = schedulerService;
            this.testQueueService = testQueueService;
            this.browserService = browserService;
            this.actionService = actionService;
        }

        /// <summary>
        /// Gets or sets the executionSequence
        /// </summary>
        private long ExecutionSequence { get; set; }

        /// <summary>
        /// process test data to generate testplan
        /// </summary>
        /// <param name="testDataDto"> the test data object</param>
        /// <param name="testQueue"> the test queue object</param>
        public void ProcessTestQueueExecutableData(IEnumerable<TblTestDataDto> testDataDto, TblTestQueueDto testQueue)
        {
            if (testDataDto != null && testDataDto.Any())
            {
                foreach (var item in testDataDto)
                {
                    switch (item.LinkTestType)
                    {
                        case (int)LinkTestType.TestStep:
                            {
                                item.ExecutionSequence = this.ExecutionSequence++;
                                this.testPlan.Add(item);
                                break;
                            }

                        case (int)LinkTestType.SqlTestStep:
                            {
                                item.ExecutionSequence = this.ExecutionSequence++;
                                this.testPlan.Add(item);
                                break;
                            }

                        case (int)LinkTestType.SharedTest:
                            {
                                var sharedSteps = item.SharedTest.SharedTestDataList.Where(x => !x.IsDeleted).OrderBy(x => x.ExecutionSequence).ToList();
                                foreach (var sharedStep in sharedSteps)
                                {
                                    var lnkSharedTestStep = item.SharedTestSteps.FirstOrDefault(x => x.SharedTestDataId == sharedStep.Id && x.IsDeleted != true);
                                    if (lnkSharedTestStep != null)
                                    {
                                        if (lnkSharedTestStep.NewOrder > 0)
                                        {
                                            sharedStep.ExecutionSequence = lnkSharedTestStep.NewOrder;
                                        }

                                        if (!string.IsNullOrEmpty(lnkSharedTestStep.NewValue))
                                        {
                                            sharedStep.Value = lnkSharedTestStep.NewValue;
                                        }

                                        sharedStep.IsIgnored = lnkSharedTestStep.IsIgnored ?? false;
                                    }
                                }

                                sharedSteps.RemoveAll(m => m.IsIgnored);
                                var mapper = this.mapperFactory.GetMapper<TblSharedTestDataDto, TblTestDataDto>();
                                var sharedStepMappedWithTestData = sharedSteps.Select(mapper.Map).OrderBy(x => x.ExecutionSequence).ToList();
                                long es = this.ExecutionSequence;
                                sharedStepMappedWithTestData.ForEach(x => { x.ExecutionSequence = es++; x.IsStepBelongsToSharedComponent = true; });
                                this.ExecutionSequence = es;
                                this.testPlan.AddRange(sharedStepMappedWithTestData);
                                break;
                            }

                        case (int)LinkTestType.SharedWebsiteTest:
                            {
                                var testData = this.table.Find(x => x.TestId == item.SharedStepWebsiteTestId && !x.IsDeleted).OrderBy(x => x.ExecutionSequence).ToList();
                                var mapper = this.mapperFactory.GetMapper<TblTestData, TblTestDataDto>();
                                var testDataDtoForSharedWebsiteTest = testData.Select(mapper.Map).OrderBy(x => x.ExecutionSequence).ToList();
                                var website = this.mapperFactory.GetMapper<TblWebsite, TblWebsiteDto>().Map(testData[0].Test.Website);
                                WebsiteUrl urlData = new WebsiteUrl();
                                if (testQueue.SuiteId.HasValue)
                                {
                                    var schedular = this.schedulerService.GetById(testQueue.SchedulerId.Value);
                                    urlData = website.WebsiteUrlList.FirstOrDefault(m => m.Id == schedular.Item.UrlId);
                                }
                                else
                                {
                                    urlData = website.WebsiteUrlList.FirstOrDefault(m => m.Id == testQueue.Settings.UrlId);
                                }

                                if (this.testPlan.Last().ActionId.Value != long.Parse(ConfigurationManager.AppSettings["IgnoreLoadNeUrlActionId"].ToString()))
                                {
                                    TblTestDataDto dummyStep = new TblTestDataDto { ActionValue = this.actionService.GetById(long.Parse(ConfigurationManager.AppSettings["LoadNewUrlActionId"].ToString())).Item.Value, ActionId = long.Parse(ConfigurationManager.AppSettings["LoadNewUrlActionId"].ToString()), Value = urlData.Url, LinkTestType = (int)LinkTestType.TestStep };
                                    testDataDtoForSharedWebsiteTest.Insert(0, dummyStep);
                                }

                                this.ProcessTestQueueExecutableData(testDataDtoForSharedWebsiteTest, testQueue);
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the suite executable data by test case identifier.
        /// </summary>
        /// <param name="testQueueId">The test queue identifier.</param>
        /// <returns>GetTestQueueExecutableData object</returns>
        public ResultMessage<TestQueue_FullTestData> GetTestQueueExecutableData(long testQueueId)
        {
            var resultMessage = new ResultMessage<TestQueue_FullTestData>();
            this.ExecutionSequence = 1;
            var testQueue = this.testQueueService.GetById(testQueueId);
            if (testQueue.Item != null)
            {
                if (testQueue.Item.SuiteId.HasValue)
                {
                    var testSuite = this.suiteService.GetById(testQueue.Item.SuiteId.Value);
                    resultMessage.Messages.AddRange(testSuite.Messages);

                    if (!resultMessage.IsError)
                    {
                        this.autoGenArray = new List<AutoGenModel>();

                        resultMessage.Item = new TestQueue_FullTestData
                        {
                            Suite = testSuite.Item
                        };

                        var testData = this.table.Find(x => x.TestId == testQueue.Item.TestId && !x.IsDeleted).OrderBy(x => x.ExecutionSequence).ToList();

                        var mapper = this.mapperFactory.GetMapper<TblTestData, TblTestDataDto>();
                        var testDataDto = testData.Select(mapper.Map).OrderBy(x => x.ExecutionSequence).ToList();
                        this.ProcessTestQueueExecutableData(testDataDto, testQueue.Item);

                        var mapperExecutableTestData = this.mapperFactory.GetMapper<TblTestDataDto, ExecutableTestData>();
                        List<ExecutableTestData> exeTestData = this.testPlan.Select(mapperExecutableTestData.Map).OrderBy(x => x.ExecutionSequence).ToList();
                        resultMessage.Item.TestData = exeTestData;

                        if (resultMessage.Item.TestData.Count > 0)
                        {
                            if (testQueue.Item.SchedulerId.HasValue)
                            {
                                var schedular = this.schedulerService.GetById(testQueue.Item.SchedulerId.Value);

                                if (!schedular.IsError)
                                {
                                    if (schedular.Item.Settings.TakeScreenShotOnUrlChangedTestId == testData[0].Test.Id)
                                    {
                                        resultMessage.Item.TakeScreenShot = true;
                                        ResultMessage<TblBrowsersDto> browser = this.browserService.GetById(schedular.Item.Settings.TakeScreenShotOnUrlChanged);
                                        resultMessage.Item.TakeScreenShotBrowser = browser.Item;
                                    }

                                    resultMessage.Item.UrlToTest = schedular.Item.Url;
                                }
                            }

                            resultMessage.Item.TestCase = this.mapperFactory.GetMapper<TblTest, TblTestDto>().Map(testData[0].Test);
                            resultMessage.Item.Website = this.mapperFactory.GetMapper<TblWebsite, TblWebsiteDto>().Map(testData[0].Test.Website);
                        }

                        resultMessage.Item.TestData.ForEach(x => x.Value = this.IsAutoGenField(x.Value));
                        this.ResolveLocatorText(resultMessage.Item.TestData);
                    }
                }
                else
                {
                    var testData = this.table.Find(x => x.TestId == testQueue.Item.TestId && !x.IsDeleted).OrderBy(x => x.ExecutionSequence).ToList();
                    var mapper = this.mapperFactory.GetMapper<TblTestData, TblTestDataDto>();
                    var testDataDto = testData.Select(mapper.Map).OrderBy(x => x.ExecutionSequence).ToList();
                    this.ProcessTestQueueExecutableData(testDataDto, testQueue.Item);

                    var mapperExecutableTestData = this.mapperFactory.GetMapper<TblTestDataDto, ExecutableTestData>();
                    this.autoGenArray = new List<AutoGenModel>();
                    List<ExecutableTestData> exeTestData = this.testPlan.Select(mapperExecutableTestData.Map).OrderBy(x => x.ExecutionSequence).ToList();
                    resultMessage.Item = new TestQueue_FullTestData
                    {
                        TestData = exeTestData
                    };

                    if (testQueue.Item.Settings.TakeScreenShotOnUrlChanged > 0)
                    {
                        resultMessage.Item.TakeScreenShot = true;
                        ResultMessage<TblBrowsersDto> browser = this.browserService.GetById(testQueue.Item.Settings.TakeScreenShotOnUrlChanged.Value);
                        resultMessage.Item.TakeScreenShotBrowser = !browser.IsError ? browser.Item : null;
                    }

                    if (resultMessage.Item.TestData.Count > 0)
                    {
                        resultMessage.Item.TestCase = this.mapperFactory.GetMapper<TblTest, TblTestDto>().Map(testData[0].Test);
                        resultMessage.Item.Website = this.mapperFactory.GetMapper<TblWebsite, TblWebsiteDto>().Map(testData[0].Test.Website);
                    }

                    resultMessage.Item.TestData.ForEach(x => x.Value = this.IsAutoGenField(x.Value));
                    this.ResolveLocatorText(resultMessage.Item.TestData);
                }

                if (resultMessage.Item != null && resultMessage.Item.Website != null && resultMessage.Item.UrlToTest.IsBlank())
                {
                    var urlData = resultMessage.Item.Website.WebsiteUrlList.FirstOrDefault(x => x.Id == testQueue.Item.Settings.UrlId);
                    if (urlData != null)
                    {
                        resultMessage.Item.UrlToTest = urlData.Url;
                    }
                }
            }

            return resultMessage;
        }

        /// <summary>
        /// Determines whether [is automatic gen field] [the specified value].
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns>string Auto-gen value</returns>
        private string IsAutoGenField(string val)
        {
            if (val.IsBlank())
            {
                return string.Empty;
            }

            string[] splittedValue = val.Split('#');

            if (splittedValue.Length > 1)
            {
                if (splittedValue[1].ToLower() == "autogen")
                {
                    var autoGenModel = new AutoGenModel();
                    if (splittedValue.Length > 3)
                    {
                        autoGenModel.AutoGenText = this.GenerateAutoString(string.Empty);
                    }
                    else
                    {
                        autoGenModel.AutoGenText = this.GenerateAutoString(splittedValue[3]);
                        autoGenModel.Prefix = splittedValue[3];
                    }

                    autoGenModel.PreviousText = splittedValue[2];
                    this.autoGenArray.Add(autoGenModel);

                    return autoGenModel.AutoGenText;
                }

                return val;
            }

            return val;
        }

        /// <summary>
        /// Generates the automatic string.
        /// </summary>
        /// <param name="preFix">The pre fix.</param>
        /// <returns>string Random value</returns>
        private string GenerateAutoString(string preFix)
        {
            char[] charArr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

            string randomString = string.Empty;

            Random objRandom = new Random();

            for (int i = 0; i < 10; i++)
            {
                int x = objRandom.Next(1, charArr.Length);

                if (!randomString.Contains(charArr.GetValue(x).ToString()))
                {
                    randomString += charArr.GetValue(x);
                }
                else
                {
                    i--;
                }
            }

            randomString = (preFix == null || preFix.Trim() == string.Empty ? "Test" : preFix) + randomString;
            return randomString;
        }

        /// <summary>
        /// Resolves the locator text.
        /// </summary>
        /// <param name="modelList">The model list.</param>
        private void ResolveLocatorText(IEnumerable<ExecutableTestData> modelList)
        {
            foreach (var item in modelList)
            {
                if (item.Value.Split('~').Length > 1)
                {
                    var resolvedLocator = this.IsInAutoGenArray(item.Value).ToList();
                    resolvedLocator.RemoveAt(0);
                    item.LocatorIdentifier = string.Format(item.LocatorIdentifier, resolvedLocator.ToArray());
                    item.Value = item.Value.Split('~')[0];
                }
                else
                {
                    item.Value = this.IsInAutoGenArray(item.Value)[0].Trim();
                }
            }
        }

        /// <summary>
        /// Determines whether [is in automatic gen array] [the specified value].
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns>string new value</returns>
        private string[] IsInAutoGenArray(string val)
        {
            foreach (var autoGen in this.autoGenArray.Where(autoGen => val.IndexOf(autoGen.PreviousText, StringComparison.InvariantCultureIgnoreCase) > -1))
            {
                return val.Replace(autoGen.PreviousText, autoGen.AutoGenText).Split('~');
            }

            return val.Split('~');
        }
    }
}