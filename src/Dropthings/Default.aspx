<%@ Page Language="C#" Culture="auto:en-US" UICulture="auto:en-US" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"  EnableSessionState="False" %>
<%@ OutputCache Location="None" NoStore="true" %>

<%@ Register Src="~/Header.ascx" TagName="Header" TagPrefix="dropthings" %>
<%@ Register Src="~/Footer.ascx" TagName="Footer" TagPrefix="dropthings" %>
<%@ Register Src="~/WidgetTabHost.ascx" TagName="WidgetTabHost" TagPrefix="dropthings" %>
<%@ Register src="~/TabBar.ascx" TagName="TabBar" TagPrefix="dropthings" %>
<%@ Register Src="~/ScriptManagerControl.ascx" TagName="ScriptManagerControl" TagPrefix="dropthings" %>
<%@ Register Src="~/TabControlPanel.ascx" TagName="TabControlPanel" TagPrefix="dropthings"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Dropthings - Free AJAX Web Portal, Web 2.0 Start Page built on ASP.NET 3.5</title>
    <meta name="Description" content="Dropthings is a free open source Web Portal. Personalize your internet experience by putting widgets on your page. It's built on ASP.NET AJAX, .NET 3.5, Windows Workflow foundation. ">
    <meta name="Keywords" content="AJAX Web Portal Web 2.0 Start Page ASP.NET 3.5 ">    
    <meta name="Page-topic" content="Free Open Source Ajax Start Page" />
    <link href="Styles/common.css" rel="Stylesheet" type="text/css" />
</head>
<body>
<form id="default_form" runat="server">
<dropthings:ScriptManagerControl ID="TheScriptManager" runat="server" />

    <div id="container">
        
        <!-- Render header first so that user can start typing search criteria while the huge runtime and other scripts download -->
        <dropthings:Header ID="Header1" runat="server" />

        <dropthings:TabBar ID="UserTabBar" runat="server" />
            
        <div id="onpage_menu">
            <div id="onpage_menu_wrapper">
                <dropthings:TabControlPanel ID="TabControlPanel" runat="server" />
            </div>
        </div>
        <div class="clear"></div>
        <div id="contents">
            <div id="contents_wrapper">
                <pre id="ErrorOnPage" enableviewstate="false" runat="server" visible="false"></pre>
        
                <div id="widget_area">
                    <div id="widget_area_wrapper">
                        <dropthings:WidgetTabHost runat="server" ID="WidgetTabHost" />                        
                    </div>
                </div>
            </div>
        </div>
                        
        <dropthings:Footer ID="Footer1" runat="server" />
                    
    </div>
    

    <!-- Fades the UI -->
    <div id="blockUI" style="display: none; background-color: black;
    width: 100%; height: 100px; position: absolute; left: 0px; top: 0px; z-index: 50000;     
    -moz-opacity:0.5;opacity:0.5;filter:alpha(opacity=50);"
    onclick="return false" onmousedown="return false" onmousemove="return false"
    onmouseup="return false" ondblclick="return false">&nbsp;</div>        
    
    <textarea id="TraceConsole" rows="10" cols="80" style="display:none"></textarea>    

    
    <!-- HTML for the delete popup dialog box -->
    <textarea id="DeleteConfirmPopupPlaceholder" style="display:none">
    &lt;div id="DeleteConfirmPopup"&gt;
        &lt;h1&gt;Delete a Widget&lt;/h1&gt;
        &lt;p&gt;Are you sure you want to delete the widget?&lt;/p&gt;
        &lt;input id="DeleteConfirmPopup_Yes" type="button" value="Yes" /&gt;&lt;input id="DeleteConfirmPopup_No" type="button" value="No" /&gt;
    &lt;/div&gt;    
    </textarea>    
    
</form>

</body>

<!-- Template for a new widget placeholder. It's used to create a fake widget
    when you drag & drop a widget from the widget gallery, until the real widget
    is loaded. -->
    <!-- Begin template -->
<script type="text/html" id="widget_template">
    <div class="widget">
        <div class="widget_header draggable">
            <table class="widget_header_table" cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td class="widget_title"><a class="widget_title_label"><!=title!></a></td>
                        <td class="widget_button"><a class="widget_close widget_box"> </a></td>
                    </tr>
                </tbody>
            </table>            
        </div>
        <div class="widget_resize_frame" >
            <div class="widget_body">
                Loading widget...
            </div>
        </div>            
    </div>    
</script>

<script type="text/javascript">
    // Ensure the framework javascripts have correctly loaded.
    if (typeof jQuery === "undefined")
        alert("jQuery is not loaded. You should check if jQuery is accessible from the internet");
    if (typeof DropthingsUI === "undefined")
        if (confirm("MyFramework.js is not loaded. Do you want to run a diagnostics?"))
            document.location = "Setup/Default.aspx";
    
</script>
</html>
