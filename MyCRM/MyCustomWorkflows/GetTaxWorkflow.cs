using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCustomWorkflows
{
    public class GetTaxWorkflow : CodeActivity
    {
        [Input ("Key")]
        public InArgument<string> Key { get; set; }

        [Output ("Tax")]
        public OutArgument<string> Tax { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            string key = Key.Get(executionContext);

            //Get data from Configurations entity
            //Call Organization WebService to retrieve the record
            QueryByAttribute query = new QueryByAttribute("new_configuration");
            query.ColumnSet = new ColumnSet(new string[] { "new_value" });
            query.AddAttributeValue("new_key", key);
            EntityCollection entityCollection = service.RetrieveMultiple(query);

            if (entityCollection.Entities.Count < 0)
            {
                tracingService.Trace("No record found for the key: {0}", key);
            }

            Entity config = entityCollection.Entities.FirstOrDefault();
            Tax.Set(executionContext, config.Attributes["new_value"].ToString());
        }
    }
}
