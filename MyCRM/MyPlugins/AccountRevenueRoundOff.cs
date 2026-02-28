using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlugins
{
    public class AccountRevenueRoundOff : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                Entity account = (Entity)context.InputParameters["Target"];

                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
                try
                {
                    tracingService.Trace(context.Depth.ToString());
                    if (context.Depth > 1)
                    {
                        return;
                    }
                    if (account.Attributes.Contains("revenue"))
                    {
                        Money revenue = (Money)account.Attributes["revenue"];
                        decimal roundedRevenue = Math.Round(revenue.Value, 1);
                        account.Attributes["revenue"] = new Money(roundedRevenue);
                    }
                }
                catch (Exception ex)
                {
                    tracingService.Trace("AccountRevenueRoundOff: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
