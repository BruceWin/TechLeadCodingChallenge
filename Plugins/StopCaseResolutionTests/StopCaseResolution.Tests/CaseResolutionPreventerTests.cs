using FakeItEasy;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using System;

namespace StopCaseResolution.Tests
{
    public class Tests
    {
        [Test]
        public void UserIsManager()
        {
            // arrange
            var userId = Guid.NewGuid();
            var roleRecord = new Entity("role");
            roleRecord["name"] = "Customer Service Manager";
            var service = A.Fake<IOrganizationService>();
            var result = new EntityCollection();
            result.Entities.Add(roleRecord);
            A.CallTo(() => service.RetrieveMultiple(A<QueryExpression>._)).Returns(result);
            var caseResolutionPreventer = new CaseResolutionPreventer(service, A.Fake<ITracingService>(), "Customer Service Manager");
            // act and assert
            Assert.DoesNotThrow(() => caseResolutionPreventer.ThrowInvalidPluginExecutionExceptionWhenForbidden(userId));
        }
        [Test]
        public void UserIsNotManager()
        {
            // arrange
            var userId = Guid.NewGuid();
            var service = A.Fake<IOrganizationService>();
            A.CallTo(() => service.Retrieve("systemuser", userId, A<ColumnSet>._)).Returns(null);
            var caseResolutionPreventer = new CaseResolutionPreventer(service, A.Fake<ITracingService>(), "Customer Service Manager");
            // act and assert
            var exception = Assert.Throws<InvalidPluginExecutionException>(() => caseResolutionPreventer.ThrowInvalidPluginExecutionExceptionWhenForbidden(userId));
            Assert.AreEqual("Case resolution permissions are missing.", exception.Message);
        }
    }
}