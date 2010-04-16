<%@ Page Language="C#" Culture="auto:en-US" UICulture="auto:en-US" AutoEventWireup="true" CodeFile="ManageAccount.aspx.cs" Inherits="ManageAccountPage" %>

<%@ Register Src="~/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Account</title>
</head>
<body bgcolor="#ffffff">
    <form id="loginform" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" ScriptMode="Release">
        </asp:ScriptManager>
        
        <script type="text/javascript" src="Scripts/MyFramework.js"></script>
        
        

        <div id="altpage">
            <div id="altpageWrapper">
                <div id="altpageContent">
                
                    <asp:HyperLink ID="GoBack" EnableViewState="false" runat="server" NavigateUrl="~/Default.aspx" >
                        &lt;&lt;&nbsp;<asp:Literal ID="Literal1" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Back%>" />
                    </asp:HyperLink>
                    
                    <br /> <br />
                    <div id="altpageHeading1">
                        <asp:Literal ID="ltlAccountSetting" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, AccountSetting%>" />
                    </div>

                    <div id="altpageHeading2">
                        <asp:Literal ID="ltlCareAboutNote" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, CareAboutNote%>" />
                    </div>
                    
                    <div id="altpageTable">
            
                        <table width="94%" align="center" cellpadding="10" cellspacing="10">
                            <tr>
                              <td colspan="2" style="white-space:nowrap;">
                                <asp:Label ID="Message" runat="server" EnableViewState="false" CssClass="altpageHeading1" />
                              </td>
                            </tr>
                            <tr>
                              <td style="white-space:nowrap;" class="align_right">
                                <asp:Label ID="lblEditPersonalInformation" EnableViewState="false" runat="server" 
                                Text="<%$Resources:SharedResources, EditPersonalInformation%>" CssClass="altpageHeading1" />
                              </td>
                              <td width="100%">&nbsp;</td>
                            </tr>
                            <tr>
                              <td width="20%" nowrap="nowrap" valign="top" class="align_right">
                                  <asp:Label ID="ltlEmail" EnableViewState="false" runat="server" 
                                  Text="<%$Resources:SharedResources, Email%>" CssClass="altpageHeading2" />
                              </td>
                              <td>
                                  <asp:TextBox ID="EmailTextbox" runat="server"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                                <td />
                                <td >
                                    <asp:Button ID="SaveButton" runat="server" Text="<%$Resources:SharedResources, Save%>" 
                                    EnableViewState="false" OnClick="SaveButton_Click" />&nbsp;
                                </td>                
                            </tr>
                            <tr>
                              <td style="white-space:nowrap;" class="align_right">
                                <asp:Label ID="ltlChangePassword" EnableViewState="false" runat="server" 
                                Text="<%$Resources:SharedResources, ChangePassword%>" CssClass="altpageHeading1" />
                              </td>
                              <td width="100%">&nbsp;</td>
                            </tr>
                            <tr>
                              <td width="20%" nowrap="nowrap" valign="top" class="align_right">
                                  <asp:Label ID="ltlOldPassword" EnableViewState="false" runat="server" 
                                  Text="<%$Resources:SharedResources, OldPassword%>" CssClass="altpageHeading2" />
                              </td>
                              <td>
                                  <asp:TextBox ID="OldPasswordTextbox" runat="server" TextMode="Password"></asp:TextBox>
                              </td>
                             </tr>
                             <tr>
                              <td width="20%" nowrap="nowrap" valign="top" class="align_right">
                                  <asp:Label ID="ltlNewPassword" EnableViewState="false" runat="server" 
                                  Text="<%$Resources:SharedResources, NewPassword%>" CssClass="altpageHeading2" />
                              </td>
                              <td>
                                  <asp:TextBox ID="NewPasswordTextbox" runat="server" TextMode="Password"></asp:TextBox>
                              </td>
                            </tr>
                            <tr>
                                <td width="20%"></td>
                                <td >
                                   <asp:Button ID="ChangePasswordButton" runat="server" Text="<%$Resources:SharedResources, Change%>" 
                                   EnableViewState="false" OnClick="ChangePasswordButton_Click" />&nbsp;
                                </td>                
                            </tr>
                        </table>                    
                    </div>
                    
                    <asp:HyperLink ID="HyperLink1" EnableViewState="false" runat="server" NavigateUrl="~/Default.aspx" >
                        &lt;&lt;&nbsp;<asp:Literal ID="Literal2" EnableViewState="false" runat="server" Text="<%$Resources:SharedResources, Back%>" />
                    </asp:HyperLink>
                

                </div>
            </div>
        </div>

        <uc2:Footer ID="Footer1" runat="server"></uc2:Footer>

    </form>
</body>
</html>
