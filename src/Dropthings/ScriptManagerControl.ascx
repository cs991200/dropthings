﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ScriptManagerControl.ascx.cs" Inherits="ScriptManagerControl" %>
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" ScriptMode="Release">
    <Services>
        <asp:ServiceReference InlineScript="true" Path="PageService.asmx" />
        <asp:ServiceReference InlineScript="true" Path="ProxyAsync.asmx" />
        <asp:ServiceReference InlineScript="true" Path="WidgetService.asmx" />
    </Services>
    <Scripts>
        <asp:ScriptReference Path="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" />
        <asp:ScriptReference Path="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.0/jquery-ui.min.js" />
        <asp:ScriptReference Path="/Scripts/jquery.micro_template.js" />
        <asp:ScriptReference Path="/Scripts/tabscroll.js" />
        <asp:ScriptReference Path="/Scripts/Myframework.js" />
        <asp:ScriptReference Path="/Scripts/Ensure.js" />
    </Scripts>    
</asp:ScriptManager>

<script type="text/javascript">if( typeof Proxy == "undefined" ) window.Proxy = Dropthings.Web.Framework.ProxyAsync;</script>
