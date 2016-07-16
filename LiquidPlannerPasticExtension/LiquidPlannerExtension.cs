using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Codice.Client.IssueTracker.LiquidPlannerExtension
{
    public class LiquidPlannerExtension : IPlasticIssueTrackerExtension
    {
        IssueTrackerConfiguration configuration;

        /// <summary>
        /// Logger manager
        /// </summary>
        internal static readonly ILog logger = LogManager.GetLogger("liquidplannerextension");

        internal static readonly string USER_KEY = "lp_userid";
        internal static readonly string BRANCH_PREFIX_KEY = "lp_branchprefix";

        internal LiquidPlannerExtension(IssueTrackerConfiguration config)
        {
            this.configuration = config;
            logger.Info("LiquidPlanner issue tracker is initialized");
        }

        #region IPlasticIssueTrackerExtension Implementation
        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public string GetExtensionName()
        {
            return "LiquidPlanner extension";
        }

        public List<PlasticTask> GetPendingTasks()
        {
            throw new NotImplementedException();
        }

        public List<PlasticTask> GetPendingTasks(string assignee)
        {
            throw new NotImplementedException();
        }

        public PlasticTask GetTaskForBranch(string fullBranchName)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, PlasticTask> GetTasksForBranches(List<string> fullBranchNames)
        {
            Dictionary<string, PlasticTask> result = new Dictionary<string, PlasticTask>();

            foreach (string fullBranchName in fullBranchNames)
            {
                string taskId = GetTaskIdFromBranchName(
                    GetBranchName(fullBranchName));
                result.Add(fullBranchName, LoadSingleTask(taskId));
            }

            return result;
        }

        public List<PlasticTask> LoadTasks(List<string> taskIds)
        {
            throw new NotImplementedException();
        }

        public void LogCheckinResult(PlasticChangeset changeset, List<PlasticTask> tasks)
        {
            throw new NotImplementedException();
        }

        public void MarkTaskAsOpen(string taskId, string assignee)
        {
            throw new NotImplementedException();
        }

        public void OpenTaskExternally(string taskId)
        {
            throw new NotImplementedException();
        }

        public bool TestConnection(IssueTrackerConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public void UpdateLinkedTasksToChangeset(PlasticChangeset changeset, List<string> tasks)
        {
            throw new NotImplementedException();
        }
        #endregion

        
        private string GetTaskIdFromBranchName(string branchName)
        {
            string prefix = configuration.GetValue(BRANCH_PREFIX_KEY);
            if (string.IsNullOrEmpty(prefix))
                return branchName;

            if (!branchName.StartsWith(prefix) || branchName == prefix)
                return string.Empty;

            return branchName.Substring(prefix.Length);
        }

        private string GetBranchName(string fullBranchName)
        {
            int lastSeparatorIndex = fullBranchName.LastIndexOf('/');

            if (lastSeparatorIndex < 0)
                return fullBranchName;

            if (lastSeparatorIndex == fullBranchName.Length - 1)
                return string.Empty;

            return fullBranchName.Substring(lastSeparatorIndex + 1);
        }

        private PlasticTask LoadSingleTask(string taskId)
        {
            throw new NotImplementedException();
        }
    }
}
