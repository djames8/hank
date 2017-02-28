﻿// ---------------------------------------------------------------------------------------------------
// <copyright file="ActionConstants.cs" company="Elephant Insurance Services, LLC">
//     Copyright (c) 2015 All Right Reserved
// </copyright>
// <author>Vyom Sharma</author>
// <date>2015-05-08</date>
// <summary>
//     The ActionConstants class
// </summary>
// ---------------------------------------------------------------------------------------------------

namespace Elephant.Hank.Resources.Json
{
    using System.Configuration;

    using Elephant.Hank.Resources.Extensions;

    /// <summary>
    /// The ActionConstants class
    /// </summary>
    public class ActionConstants
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static ActionConstants instance;

        /// <summary>
        /// Prevents a default instance of the <see cref="ActionConstants"/> class from being created.
        /// </summary>
        private ActionConstants()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static ActionConstants Instance
        {
            get
            {
                return instance ?? (instance = new ActionConstants());
            }
        }

        /// <summary>
        /// Gets the SetVariable Action's Identifier
        /// </summary>
        public long SetVariableActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["SetVariableActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the SetVariable Action's Identifier
        /// </summary>
        public long SetVariableManuallyActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["SetVariableManuallyActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the DeclareVariable Action's Identifier
        /// </summary>
        public long DeclareVariableActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["DeclareVariableActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the TakeScreenShot Action's Identifier
        /// </summary>
        public long TakeScreenShotActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["TakeScreenShotActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the LoadNewUr Action's Identifier
        /// </summary>
        public long LoadNewUrlActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["LoadNewUrlActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the SwitchWebsiteType Action's Identifier
        /// </summary>
        public long SwitchWebsiteTypeActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["SwitchWebsiteTypeActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the AssertUrlToContain Action's Identifier
        /// </summary>
        public long AssertUrlToContainActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["AssertUrlToContainActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the HandleBrowserAlertPopup Action's Identifier
        /// </summary>
        public long HandleBrowserAlertPopupActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["HandleBrowserAlertPopupActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the Wait Action's Identifier
        /// </summary>
        public long WaitActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["WaitActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the Load Partial Url Action's Identifier
        /// </summary>
        public long LoadPartialUrlActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["LoadPartialUrlActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the Log Text Action's Identifier
        /// </summary>
        public long LogTextActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["LogTextActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the Log Text Action's Identifier
        /// </summary>
        public long AssertToEqualActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["AssertToEqualActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the assert to equal ignore case action identifier.
        /// </summary>
        public long AssertToEqualIgnoreCaseActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["AssertToEqualIgnoreCaseActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the Switch window Action's Identifier
        /// </summary>
        public long SwitchWindowActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["SwitchWindowActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the Switch window Action's Identifier
        /// </summary>
        public long IgnoreLoadNeUrlActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["IgnoreLoadNeUrlActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the sen key action identifier.
        /// </summary>
        /// <value>
        /// The sen key action identifier.
        /// </value>
        public long SendKeyActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["SendKeyActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the terminate test action id
        /// </summary>
        public long TerminateTestActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["TerminateTestActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the terminate test action id
        /// </summary>
        public long AssertToContainActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["AssertToContainActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the assert to contain ignore case action identifier.
        /// </summary>
        public long AssertToContainIgnoreCaseActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["AssertToContainIgnoreCaseActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the switch to frame action identifier.
        /// </summary>
        /// <value>
        /// The switch to frame action identifier.
        /// </value>
        public long SwitchFrameActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["SwitchToFrameActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the switch to default content action identifier.
        /// </summary>
        /// <value>
        /// The switch to default content action identifier.
        /// </value>
        public long SwitchToDefaultContentActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["SwitchToDefaultContentActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the load report data action identifier.
        /// </summary>
        /// <value>
        /// The load report data action identifier.
        /// </value>
        public long LoadReportDataActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["LoadReportDataActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the mark load data from report.
        /// </summary>
        /// <value>
        /// The mark load data from report.
        /// </value>
        public long MarkLoadDataFromReportActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["MarkLoadDataFromReportActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the set calendar date action identifier.
        /// </summary>
        /// <value>
        /// The set calendar date action identifier.
        /// </value>
        public long SetCalendarDateActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["SetCalendarDateActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the read attribute action identifier.
        /// </summary>
        /// <value>
        /// The read attribute action identifier.
        /// </value>
        public long ReadAttributeActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["ReadAttributeActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the read attribute action identifier.
        /// </summary>
        /// <value>
        /// The read attribute action identifier.
        /// </value>
        public long OpenBrowserActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["OpenBrowserActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the read attribute action identifier.
        /// </summary>
        /// <value>
        /// The read attribute action identifier.
        /// </value>
        public long CloseBrowserActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["CloseBrowserActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the transformation on action identifier.
        /// </summary>
        /// <value>
        /// The transformation on action identifier.
        /// </value>
        public long TransformationOnActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["TransformationOnActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the transformation off action identifier.
        /// </summary>
        /// <value>
        /// The transformation off action identifier.
        /// </value>
        public long TransformationOffActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["TransformationOffActionId"].ToInt64();
            }
        }

        /// <summary>
        /// Gets the close current tab action identifier.
        /// </summary>
        /// <value>
        /// The close current tab action identifier.
        /// </value>
        public long CloseCurrentTabActionId
        {
            get
            {
                return ConfigurationManager.AppSettings["CloseCurrentTabActionId"].ToInt64();
            }
        }
    }
}
