using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlugins
{
    public class PreEntityImageDemo : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));
            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];
                // Obtain the IOrganizationService instance which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
                try
                {
                    // Plug-in business logic goes here.
                    //if (context.PreEntityImages.Contains("PreImage") && 
                    //    context.PreEntityImages["PreImage"] is Entity)
                    //{
                    //    Entity preImage = context.PreEntityImages["PreImage"];
                    //    string firstName = string.Empty;
                    //    if (preImage.Attributes.Contains("firstname"))
                    //    {
                    //        firstName = preImage.Attributes["firstname"].ToString();
                    //    }
                    //    string lastName = string.Empty;
                    //    if (preImage.Attributes.Contains("lastname"))
                    //    {
                    //        lastName = preImage.Attributes["lastname"].ToString();
                    //    }
                    //    tracingService.Trace("Pre-Entity Image - First Name: {0}, Last Name: {1}", firstName, lastName);

                    string modifiedPhone = entity.Attributes["telephone1"].ToString();
                    Entity preImage = (Entity)context.PreEntityImages["PreImage"];
                    string originalPhone = preImage.Attributes["telephone1"].ToString();

                    throw new InvalidPluginExecutionException(string.Format("Original Phone: {0}, Modified Phone: {1}", originalPhone, modifiedPhone));
                }
                catch (Exception ex)
                {
                    tracingService.Trace("PreEntityImageDemo: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}