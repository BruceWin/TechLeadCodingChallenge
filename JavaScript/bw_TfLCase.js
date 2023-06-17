var TfL = TfL || {};
TfL.Case = {
    escalate: async function escalate(formContext) {
        var caseId = formContext.data.entity.getId().replace("}","").replace("{","");
        var confirmed = await Xrm.Navigation.openConfirmDialog({ title: "Confirm Case Escalation" });
        if (confirmed) 
            TfL.Case.createFollowUp(caseId, "escalation");
    },
    async createFollowUp(caseId, followUpType) {
        var incident = await Xrm.WebApi.retrieveRecord("incident", caseId);
        var followUpPayload = {
            "bw_CaseId@odata.bind": `/incidents(${caseId})`,
            "bw_name": incident.title,
            "bw_description": incident.description

        };
        var followUp = await Xrm.WebApi.createRecord("bw_followup", followUpPayload);

        
    },
    addFollowUpDocumentsFromCase(caseId) {
        Xrm.WebApi.retrieveMultipleRecords("annotation",
            `?$filter=_objectid_value eq ${caseId} and documentbody ne null`
        ).then(function (success) {
            console.log(success);
        });
    }
};
