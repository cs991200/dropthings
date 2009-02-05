<%@ Application Language="C#" %>
<%@ Import Namespace="Dropthings.Business" %>
<%@ Import Namespace="Dropthings.Business.Workflows" %>
<%@ Import Namespace="Dropthings.Business.Container" %>
<%@ Import Namespace="System.Workflow.Runtime" %>
<%@ Import Namespace="Dropthings.Business.Workflows.UserAccountWorkflow" %>
<%@ Import Namespace="Dropthings.Business.Workflows.SystemWorkflows" %>

<script RunAt="server">
    // Copyright (c) Omar AL Zabir. All rights reserved.
    // For continued development and updates, visit http://msmvps.com/omar

    const string APPLICATION_WORKFLOW_RUNTIME_KEY = "GlobalSynchronousWorkflowRuntime";
    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup

        // Create a global workflow runtime and store in Application context.
        System.Workflow.Runtime.WorkflowRuntime runtime = Dropthings.Business.Workflows.WorkflowHelper.CreateDefaultRuntime();
        Application[APPLICATION_WORKFLOW_RUNTIME_KEY] = runtime;

        // Setup default Dependencies for regular execution where all dependencies are real
        Dropthings.Business.Container.ObjectContainer.SetupDefaults(runtime);
        SetupDefaultSetting();
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

        // Terminate the workflow runtime
        System.Workflow.Runtime.WorkflowRuntime runtime = Application[APPLICATION_WORKFLOW_RUNTIME_KEY] as System.Workflow.Runtime.WorkflowRuntime;
        if (null != runtime)
            Dropthings.Business.Workflows.WorkflowHelper.TerminateDefaultRuntime(runtime);

        // Teardown Dependency Containers
        Dropthings.Business.Container.ObjectContainer.Dispose();
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        if (null != Context && null != Context.AllErrors)
            System.Diagnostics.Debug.WriteLine(Context.AllErrors.Length);
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "GET")
        {
            if (Request.AppRelativeCurrentExecutionFilePath.EndsWith(".aspx"))
            {
                Response.Filter = new Dropthings.Web.Util.ScriptDeferFilter(Response);

                Response.Filter = new Dropthings.Web.Util.StaticContentFilter(Response,
                    ConfigurationManager.AppSettings["ImgPrefix"],
                    ConfigurationManager.AppSettings["JsPrefix"],
                    ConfigurationManager.AppSettings["CssPrefix"]);
            }
        }
    }

    private static void SetupDefaultSetting()
    {
        //setup default roles, template user and role template      
        RunWorkflow.Run<SetupDefaultRolesWorkflow, SetupDefaultRolesWorkflowRequest, SetupDefaultRolesWorkflowResponse>(
            new SetupDefaultRolesWorkflowRequest { }
        );

        Dropthings.Business.UserSettingTemplateSettingsSection settings = (UserSettingTemplateSettingsSection)ConfigurationManager.GetSection(UserSettingTemplateSettingsSection.SectionName);

        foreach (UserSettingTemplateElement setting in settings.UserSettingTemplates)
        {
            RunWorkflow.Run<CreateTemplateUserWorkflow, CreateTemplateUserWorkflowRequest, CreateTemplateUserWorkflowResponse>(
                new CreateTemplateUserWorkflowRequest { Email = setting.UserName, IsActivationRequired = false, Password = setting.Password, RequestedUsername = setting.UserName, RoleName = setting.RoleNames, TemplateRoleName = setting.TemplateRoleName }
            );

        }
    }
</script>

