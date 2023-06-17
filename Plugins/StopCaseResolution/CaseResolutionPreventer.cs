using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StopCaseResolution
{
    public class CaseResolutionPreventer
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly string _requiredRoleForCaseResolution;
        public CaseResolutionPreventer(IOrganizationService service, ITracingService tracingService, string requiredRoleForCaseResolution)
        {
            this._service = service;
            this._tracingService = tracingService;
            this._requiredRoleForCaseResolution = requiredRoleForCaseResolution;
        }
        public void ThrowInvalidPluginExecutionExceptionWhenForbidden(Guid userId)
        {
            var query = new QueryExpression("role")
            {
                ColumnSet = new ColumnSet("name"),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = "name",
                            Operator = ConditionOperator.Equal,
                            Values = { _requiredRoleForCaseResolution }
                        }
                    }
                },
                LinkEntities =
                {
                    new LinkEntity
                    {
                        LinkFromEntityName = "role",
                        LinkFromAttributeName = "roleid",
                        LinkToEntityName = "systemuserroles",
                        LinkToAttributeName = "roleid",
                        Columns = new ColumnSet(false),
                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression
                                {
                                    AttributeName = "systemuserid",
                                    Operator = ConditionOperator.Equal,
                                    Values = { userId }
                                }
                            }
                        }
                    }
                }
            };
            var result = _service.RetrieveMultiple(query);
            if (result.Entities.Count == 0)
            {
                _tracingService.Trace($"user {userId} does not have the {_requiredRoleForCaseResolution} role.");
                throw new InvalidPluginExecutionException("Case resolution permissions are missing.");
            }
            _tracingService.Trace($"user {userId} has the {_requiredRoleForCaseResolution} role.");
        }
    }
}
