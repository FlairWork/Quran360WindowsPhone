using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Quran360
{
	public static class AppModel
	{
		public static int PROJECT_STATUS_NOT_STARTED_ID = 0;
		public static int PROJECT_STATUS_ACTIVE_ID = 1;
		public static int PROJECT_STATUS_COMPLETED_ID = 2;
		public static string ASSETS_FOLDER = "Quran360Assets";

        /*
		public static List<ProjectStatus> ProjectStatuses
		{
			get
			{
				List<ProjectStatus> projectStatuses;
				projectStatuses = new List<ProjectStatus>();
				projectStatuses.Add(new ProjectStatus() { Name = "not started", Id = AppModel.PROJECT_STATUS_NOT_STARTED_ID });
				projectStatuses.Add(new ProjectStatus() { Name = "active", Id = AppModel.PROJECT_STATUS_ACTIVE_ID });
				projectStatuses.Add(new ProjectStatus() { Name = "completed", Id = AppModel.PROJECT_STATUS_COMPLETED_ID });
				return projectStatuses;
			}
		}*/

		public static Uri TransactionDoneNextPage
		{
			get;
			set;
		}

		public static Uri ProjectDoneNextPage
		{
			get;
			set;
		}
	}
}
