﻿namespace Dropthings.Business.Activities.PageActivities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Linq;
    using System.Workflow.Activities;
    using System.Workflow.Activities.Rules;
    using System.Workflow.ComponentModel;
    using System.Workflow.ComponentModel.Compiler;
    using System.Workflow.ComponentModel.Design;
    using System.Workflow.ComponentModel.Serialization;
    using System.Workflow.Runtime;

    using Dropthings.DataAccess;

    public partial class ApplyColumnWidthsActivity : Activity
    {
        #region Fields

        // Using a DependencyProperty as the backing store for ColumnWidths.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnWidthsProperty = 
            DependencyProperty.Register("ColumnWidths", typeof(int[]), typeof(ApplyColumnWidthsActivity));

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty = 
            DependencyProperty.Register("Columns", typeof(List<Column>), typeof(ApplyColumnWidthsActivity));

        // Using a DependencyProperty as the backing store for PageId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIdProperty = 
            DependencyProperty.Register("PageId", typeof(int), typeof(ApplyColumnWidthsActivity));

        #endregion Fields

        #region Constructors

        public ApplyColumnWidthsActivity()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public int[] ColumnWidths
        {
            get { return (int[])GetValue(ColumnWidthsProperty); }
            set { SetValue(ColumnWidthsProperty, value); }
        }

        public List<Column> Columns
        {
            get { return (List<Column>)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public int PageId
        {
            get { return (int)GetValue(PageIdProperty); }
            set { SetValue(PageIdProperty, value); }
        }

        #endregion Properties

        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var columns = DatabaseHelper.GetList<Column, int>(DatabaseHelper.SubsystemEnum.Page,
                this.PageId,
                LinqQueries.CompiledQuery_GetColumnsByPageId);

            this.Columns = columns;

            DatabaseHelper.UpdateList<Column>(DatabaseHelper.SubsystemEnum.Column, columns,
                null,
                (col) => col.ColumnWidth = this.ColumnWidths[col.ColumnNo]);

            return ActivityExecutionStatus.Closed;
        }

        #endregion Methods
    }
}