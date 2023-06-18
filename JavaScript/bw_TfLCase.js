var TfL = TfL || {};
TfL.Case = {
    escalate: async function escalate(formContext) {
        var caseId = formContext.data.entity.getId().replace("}", "").replace("{", "");
        var confirmed = await Xrm.Navigation.openConfirmDialog({ title: "Confirm Case Escalation Follow Up" });
        if (confirmed)
            TfL.Case.createFollowUp(caseId, "escalation");
    },
    confidential: async function confidential(formContext) {
        var caseId = formContext.data.entity.getId().replace("}", "").replace("{", "");
        var confirmed = await Xrm.Navigation.openConfirmDialog({ title: "Confirm Confidential Case Follow Up" });
        if (confirmed)
            TfL.Case.createFollowUp(caseId, "confidential");
    },
    createFollowUp: async function createFollowUp(caseId, followUpType) {
        var incident = await Xrm.WebApi.retrieveRecord("incident", caseId);
        var followUpPayload = {
            "bw_CaseId@odata.bind": `/incidents(${caseId})`,
            "bw_name": incident.title,
            "bw_description": incident.description,
            "bw_type": followUpType == "escalation" ? 121330000 : 121330001

        };
        try {
            await Xrm.WebApi.createRecord("bw_followup", followUpPayload);
        }
        catch (e) {
            if (e.errorCode === 2147746336) {
                Xrm.Navigation.openAlertDialog({ text: "Insufficient permissions to create a follow up.", title: "Insufficient permissions" });
            }
            else {
                Xrm.Navigation.openAlertDialog({ text: "Follow up creation failed. Try again or contact support.", title: "Follow up creation failed" });
            }
        }
    }
};