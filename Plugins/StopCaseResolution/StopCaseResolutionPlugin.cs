using System;

namespace StopCaseResolution
{
    public class StopCaseResolutionPlugin : PluginBase
    {
        private readonly string _unsecureConfiguration;
        public StopCaseResolutionPlugin(string unsecureConfiguration, string secureConfiguration) : base(typeof(StopCaseResolutionPlugin)) 
        {
            this._unsecureConfiguration = unsecureConfiguration;
        }

        protected override void ExecuteDataversePlugin(ILocalPluginContext localPluginContext)
        {
            if (localPluginContext == null)
            {
                throw new ArgumentNullException(nameof(localPluginContext));
            }

            var caseResolutionPreventer = new CaseResolutionPreventer(localPluginContext.InitiatingUserService, localPluginContext.TracingService, _unsecureConfiguration);
            caseResolutionPreventer.ThrowInvalidPluginExecutionExceptionWhenForbidden(localPluginContext.PluginExecutionContext.InitiatingUserId);
        }
    }
}