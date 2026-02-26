using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace MyPlugins
{
    public class TaskCreate : IPlugin
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
                Entity contact = (Entity)context.InputParameters["Target"];

                // Obtain the IOrganizationService instance which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    // Plug-in business logic goes here.

                    //create a new record of type task
                    Entity taskRecord = new Entity("task");

                    //single line of text attributes
                    taskRecord.Attributes.Add("subject", "Follow Up");
                    taskRecord.Attributes.Add("description", "Please follow up with the contact.");

                    //date attribute
                    taskRecord.Attributes.Add("scheduledend", DateTime.Now.AddDays(2));

                    //option set attribute set as "High"
                    taskRecord.Attributes.Add("prioritycode", new OptionSetValue(2));

                    //parent contact lookup attribute
                    //taskRecord.Attributes.Add("regardingobjectid", new EntityReference("contact", contact.Id));
                    taskRecord.Attributes.Add("regardingobjectid", contact.ToEntityReference());

                    //create the task record in CRM
                    Guid taskGuid = service.Create(taskRecord);
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FollowUpPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }

    }
}
