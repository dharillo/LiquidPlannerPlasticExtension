using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using LiquidPlannerPlasticExtension.LiquidPlanner;
using System.Diagnostics;
using Codice.Utils;

namespace Codice.Client.IssueTracker.LiquidPlannerExtension
{
    public class LiquidPlannerExtension : IPlasticIssueTrackerExtension
    {
        IssueTrackerConfiguration configuration;
        LiquidPlannerConnection connection;
        Member userInfo;

        /// <summary>
        /// Logger manager
        /// </summary>
        internal static readonly ILog logger = LogManager.GetLogger("liquidplannerextension");

        internal static readonly string USER_KEY = "User";
        internal static readonly string PASSWORD_KEY = "Password";
        internal static readonly string BRANCH_PREFIX_KEY = "Branch prefix";
        internal static readonly string WORKSPACE_KEY = "Workspace";

        internal LiquidPlannerExtension(IssueTrackerConfiguration config)
        {
            this.configuration = config;
            this.userInfo = null;
            this.connection = null;

            logger.Info("LiquidPlanner issue tracker is initialized");
        }

        #region IPlasticIssueTrackerExtension Implementation
        public void Connect()
        {
            string user = configuration.GetValue(USER_KEY);
            string password = configuration.GetValue(PASSWORD_KEY);
            string workspaceId = configuration.GetValue(WORKSPACE_KEY);

            connection = new LiquidPlannerConnection(user,GetDecryptedPassword(password));

            connection.WorkspaceId = Convert.ToInt32(workspaceId);

            userInfo = connection.GetAccount();
        }

        public void Disconnect()
        {
            connection = null;
        }

        public string GetExtensionName()
        {
            return "LiquidPlanner extension";
        }

        public List<PlasticTask> GetPendingTasks()
        {
            // FIXME Should return the task of all the members in the workspace.
            return GetPendingTasks(userInfo.Email);
        }

        public List<PlasticTask> GetPendingTasks(string assignee)
        {
            Member assigneeMember = userInfo;
            if (assignee != userInfo.Email)
            {
                assigneeMember = connection.GetMemberInfo(assignee);
            }
            List<Item> tasks = connection.GetNotClosedTasks(assigneeMember.Id);

            return tasks.Select((task) => BuildPlasticTask(task)).ToList();
        }

        public PlasticTask GetTaskForBranch(string fullBranchName)
        {
            string taskId = GetTaskIdFromBranchName(GetBranchName(fullBranchName));
            return LoadSingleTask(taskId);
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
            var plasticTasks = new List<PlasticTask>();
            foreach(var taskId in taskIds)
            {
                plasticTasks.Add(LoadSingleTask(taskId));
            }
            return plasticTasks;
        }

        public void LogCheckinResult(PlasticChangeset changeset, List<PlasticTask> tasks)
        {
            string newComment = string.Format("Checkin repository: {0}<br />Checkin ID: {1}<br />Checkin GUID: {2}<br />Checkin comment:<br /><p>{3}</p>",
                changeset.Repository, changeset.Id, changeset.Guid, changeset.Comment);

            foreach(var task in tasks)
            {
                connection.CreateComment(task.Id, newComment);
            }
        }

        public void MarkTaskAsOpen(string taskId, string assignee)
        {
            throw new NotImplementedException();
        }

        public void OpenTaskExternally(string taskId)
        {
            string workspaceId = configuration.GetValue(WORKSPACE_KEY);
            Process.Start(string.Format("https://app.liquidplanner.com/space/{0}/projects/show/{1}", workspaceId, taskId));
        }

        public bool TestConnection(IssueTrackerConfiguration configuration)
        {
            string user = configuration.GetValue(USER_KEY);
            string password = configuration.GetValue(PASSWORD_KEY);
            string workspaceId = configuration.GetValue(WORKSPACE_KEY);
            var testConnection = new LiquidPlannerConnection(user, GetDecryptedPassword(password));

            Member account = null;
            try
            {
                account = testConnection.GetAccount(); 
            }
            catch
            {
                return false;
            }
            return account != null;
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
            if (string.IsNullOrEmpty(taskId))
                return null;

            Item task = connection.GetTask(Convert.ToInt32(taskId));
            return BuildPlasticTask(task);
        }

        private PlasticTask BuildPlasticTask(Item task)
        {
            if (task == null)
                return null;

            Member creatorInfo = connection.GetMemberInfo(task.CreatorId);
            return new PlasticTask()
            {
                Description = task.Description,
                Title = task.Name,
                Status = task.GetStatus(),
                Id = Convert.ToString(task.Id),
                Owner = /*task.CreatorId.ToString()*/creatorInfo.Name
            };
        }

        private string GetDecryptedPassword(string encryptedPassword)
        {
            return CryptoServices.GetDecryptedPassword(encryptedPassword);
        }
    }
}
