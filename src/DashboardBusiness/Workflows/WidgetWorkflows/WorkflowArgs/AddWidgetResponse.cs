﻿namespace Dropthings.Business.Workflows.WidgetWorkflows.WorkflowArgs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Dropthings.DataAccess;

    public class AddWidgetResponse : UserWorkflowResponseBase
    {
        #region Properties

        public WidgetInstance NewWidget
        {
            get; set;
        }

        #endregion Properties
    }
}