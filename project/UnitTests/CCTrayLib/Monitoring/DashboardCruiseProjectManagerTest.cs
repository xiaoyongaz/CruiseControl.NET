using System;
using NMock;
using NUnit.Framework;
using ThoughtWorks.CruiseControl.CCTrayLib.Monitoring;
using ThoughtWorks.CruiseControl.Remote;

namespace ThoughtWorks.CruiseControl.UnitTests.CCTrayLib.Monitoring
{
	[TestFixture]
	public class DashboardCruiseProjectManagerTest
	{
		private readonly Uri severUri = new Uri("http://xxx");
		private DynamicMock mockWebRetriever;
		private DynamicMock mockDashboardXmlParser;
		private DashboardCruiseProjectManager manager;

		[SetUp]
		public void SetUp()
		{
			mockWebRetriever = new DynamicMock(typeof (IWebRetriever));
			mockDashboardXmlParser = new DynamicMock(typeof(IDashboardXmlParser));
			IWebRetriever webRetriever = (IWebRetriever) mockWebRetriever.MockInstance;
			IDashboardXmlParser dashboardXmlParser = (IDashboardXmlParser) mockDashboardXmlParser.MockInstance;
			manager = new DashboardCruiseProjectManager(webRetriever, dashboardXmlParser, severUri, "yyy");			
		}
		
		[Test]
		[ExpectedException(typeof (NotImplementedException), "Force build not currently supported on dashboard projects")]
		public void ForceBuildThrowsAnNotImplementedException()
		{
			manager.ForceBuild();
		}

		[Test]
		public void ProjectStatusRetrivesDocumentViaHttpAndParsesReturnedXml()
		{
			ProjectStatus expected = new ProjectStatus();
			const string xmlContent = "<Projects />";
			mockWebRetriever.ExpectAndReturn("Get", xmlContent, severUri);
			mockDashboardXmlParser.ExpectAndReturn("ExtractAsProjectStatus", expected, xmlContent, "yyy" );
			ProjectStatus actual = manager.ProjectStatus;
			
			Assert.AreSame(expected, actual);
			mockWebRetriever.Verify();
			mockDashboardXmlParser.Verify();
		}
	}
}