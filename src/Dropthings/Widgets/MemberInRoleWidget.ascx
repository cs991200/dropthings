﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MemberInRoleWidget.ascx.cs" Inherits="Widgets_MemberInRoleWidget" %>
<asp:ObjectDataSource ID="odsMembers" runat="server" EnablePaging="True" 
    SelectCountMethod="GetMemberCountByRole" 
    SelectMethod="GetPagedMemberByRole"
    TypeName="Dropthings.Business.Facade.Facade">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlRole" Name="roleName" PropertyName="SelectedValue" Type="String" />        
    </SelectParameters>
</asp:ObjectDataSource>
<asp:Panel ID="SettingsPanel" runat="server" Visible="false">
</asp:Panel>
<div>
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>
<asp:MultiView ID="Multiview" runat="server" ActiveViewIndex="0">
     <asp:View runat="server" ID="RoleListView">
        <div style="float:right; margin:5px;clear:both">
            <asp:ImageButton ID="btnAdd" runat="server" SkinID="sknImageAdd" ToolTip="Add Member" OnClick="AddNewMemberClicked"></asp:ImageButton>
        </div>
        <div>
            <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectionChanged"></asp:DropDownList>
        </div>
        <div class="grid">
            <asp:ListView ID="lvItems" DataSourceID="odsMembers" runat="server" OnItemCommand="ItemCommand">
                <LayoutTemplate>                                         
                    <table id="MemberList" runat="server" class="datatable" cellpadding="0" cellspacing="0">
                        <tr>
                            <th class="first">
                                UserName
                            </th>
                            <th>IsActive</th>
                            <th></th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server" />
                    </table>  
                    <div class="pager">
                        <asp:DataPager ID="pagerBottom" runat="server" PageSize="1">
                            <Fields>
                                <asp:TemplatePagerField><PagerTemplate></PagerTemplate></asp:TemplatePagerField>
                                <asp:NextPreviousPagerField ButtonCssClass="command" FirstPageText="«" PreviousPageText="‹" RenderDisabledButtonsAsLabels="true" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" /> 
                                <asp:NumericPagerField ButtonCount="7" NumericButtonCssClass="command" CurrentPageLabelCssClass="current" NextPreviousButtonCssClass="command" />
                                <asp:NextPreviousPagerField ButtonCssClass="command" LastPageText="»" NextPageText="›" RenderDisabledButtonsAsLabels="true" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" />                                            
                            </Fields>
                        </asp:DataPager>
                    </div>                                                  
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="item" runat="server" class="row">
                        <td><%# Eval("aspnet_User.Username")%></td>
                        <td><%# Convert.ToBoolean(Eval("IsApproved")) ? "Yes" : "No"%></td>
                        <td style="text-align: left; white-space: nowrap;">
                            <asp:ImageButton ID="btnDelete" runat="server" SkinID="sknImageDelete" ToolTip="Delete" CommandName="DeleteItem" CommandArgument=<%# Eval("aspnet_User.Username")%>></asp:ImageButton>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
     </asp:View>
     <asp:View runat="server" ID="MemberEditor">
        <div class="form">
            <p>
                <asp:Label ID="ltlUsername" EnableViewState="false" runat="server" Text="Username" />
                <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="ltlPassword" EnableViewState="false" runat="server" Text="Password" />
                <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="ltlEmail" EnableViewState="false" runat="server" Text="Email" />
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" Text="IsActive" />
            </p>
            <p>
                <asp:Button ID="btnCancel" runat="server" Text="<%$Resources:SharedResources, Cancel%>" EnableViewState="false" OnClick="CancelClicked" />
                <asp:Button ID="btnSubmit" runat="server" Text="<%$Resources:SharedResources, Submit%>" EnableViewState="false" OnClick="SaveClicked" />
            </p>
        </div>
     </asp:View>
</asp:MultiView>