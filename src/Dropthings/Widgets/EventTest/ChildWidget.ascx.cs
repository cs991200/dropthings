﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Dropthings.Widget.Framework;

public partial class Widgets_EventTest_ChildWidget : System.Web.UI.UserControl, IWidget
{
    #region Fields

    private IWidgetHost _Host;

    #endregion Fields

    #region Methods

    public void AcceptEvent(object sender, EventArgs e)
    {
        if (sender != this && e is MasterChildEventArgs)
        {
            var arg = e as MasterChildEventArgs;
            this.Received.Text = arg.Who + " says, " + arg.Message;
            _Host.Refresh(this);
        }
    }

    public void Closed()
    {
    }

    public void Collasped()
    {
    }

    public void Expanded()
    {
    }

    public void HideSettings(bool userClicked)
    {
    }

    public new void Init(IWidgetHost host)
    {
        _Host = host;
        host.EventBroker.AddListener(this);
    }

    public void Maximized()
    {
    }

    public void Restored()
    {
    }

    public void ShowSettings(bool userClicked)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Raise_Clicked(object sender, EventArgs e)
    {
        MasterChildEventArgs args = new MasterChildEventArgs("Child " + _Host.WidgetInstance.Id, this.Message.Text);
        _Host.EventBroker.RaiseEvent(this, args);
    }

    #endregion Methods
}