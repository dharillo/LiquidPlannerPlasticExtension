using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codice.Client.IssueTracker.LiquidPlannerExtension
{
    class LiquidPlannerExtensionFactory : IPlasticIssueTrackerExtensionFactory
    {
        #region IPlasticIssueTrackerExtensionFactory implementation
        public IssueTrackerConfiguration GetConfiguration(IssueTrackerConfiguration storedConfiguration)
        {
            List<IssueTrackerConfigurationParameter> parameters = new List<IssueTrackerConfigurationParameter>();

            ExtensionWorkingMode workingMode = GetWorkingMode(storedConfiguration);
            AddUserParam(storedConfiguration, parameters);
            AddBranchPrefixParam(storedConfiguration, parameters);

            return new IssueTrackerConfiguration(workingMode, parameters);
        }

        public IPlasticIssueTrackerExtension GetIssueTrackerExtension(IssueTrackerConfiguration configuration)
        {
            return new LiquidPlannerExtension(configuration);
        }

        public string GetIssueTrackerName()
        {
            return "LiquidPlanner Issue Tracker";
        }
        #endregion
        
        /// <summary>
        /// Creates a valid value for the working mode of the extension if the given
        /// configuration doesn't have one. The default value for the working mode
        /// is <see cref="ExtensionWorkingMode.TaskOnBranch"/>.
        /// </summary>
        /// <param name="config">Configuration to check.</param>
        /// <returns>Valid value for the extension working mode.</returns>
        private ExtensionWorkingMode GetWorkingMode(IssueTrackerConfiguration config)
        {
            if (config == null)
                return ExtensionWorkingMode.TaskOnBranch;

            if (config.WorkingMode == ExtensionWorkingMode.None)
                return ExtensionWorkingMode.TaskOnBranch;

            return config.WorkingMode;
        }

        /// <summary>
        /// Creates a valid value for the userId configuration parameter and adds it to the list
        /// of parameters given.
        /// </summary>
        /// <param name="storedConfiguration">Configuration which parameter value must be checked
        /// and changed if necessary.</param>
        /// <param name="parameters">List of parameters being build with all the valid parameters.</param>
        private static void AddUserParam(IssueTrackerConfiguration storedConfiguration, List<IssueTrackerConfigurationParameter> parameters)
        {
            string user = GetValidParameterValue(storedConfiguration, LiquidPlannerExtension.USER_KEY, "username");
            IssueTrackerConfigurationParameter userIdParam = new IssueTrackerConfigurationParameter()
            {
                Name = LiquidPlannerExtension.USER_KEY,
                Value = user,
                Type = IssueTrackerConfigurationParameterType.User,
                IsGlobal = false
            };
            parameters.Add(userIdParam);
        }

        /// <summary>
        /// Creates a valid value for the branchPrefix configuration parameter and adds it to the list
        /// of parameters given.
        /// </summary>
        /// <param name="storedConfiguration">Configuration which parameter value must be checked
        /// and changed if necessary.</param>
        /// <param name="parameters">List of parameters being build with all the valid parameters.</param>
        private static void AddBranchPrefixParam(IssueTrackerConfiguration storedConfiguration, List<IssueTrackerConfigurationParameter> parameters)
        {
            string prefix = GetValidParameterValue(storedConfiguration, LiquidPlannerExtension.BRANCH_PREFIX_KEY, "lptask_");
            IssueTrackerConfigurationParameter branchPrefix = new IssueTrackerConfigurationParameter()
            {
                Name = LiquidPlannerExtension.BRANCH_PREFIX_KEY,
                Value = prefix,
                Type = IssueTrackerConfigurationParameterType.BranchPrefix,
                IsGlobal = true
            };
            parameters.Add(branchPrefix);
        }

        /// <summary>
        /// Gets a valid value for the parameter indicated. This means that it checks if the
        /// given configuration has a valid value and, if not, returns the default value for
        /// that parameter.
        /// </summary>
        /// <param name="config">Configuration to check</param>
        /// <param name="key">Key of the parameter to check.</param>
        /// <param name="defaultValue">Default value to use if the configuration doesn't have a valid
        /// value.</param>
        /// <returns>Valid value for the parameter.</returns>
        private static string GetValidParameterValue(IssueTrackerConfiguration config, string key, string defaultValue)
        {
            string configValue = (config != null) ? config.GetValue(key) : null;
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }
    }
}
