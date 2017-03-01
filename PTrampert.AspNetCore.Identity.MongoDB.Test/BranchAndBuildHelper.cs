using System;
using System.Collections.Generic;
using System.Text;

namespace PTrampert.AspNetCore.Identity.MongoDB.Test
{
    public class BranchAndBuildHelper
    {
        public static string BranchAndBuild
        {
            get
            {
                var branch = Environment.GetEnvironmentVariable("BRANCH_NAME") ?? "local";
                var build = Environment.GetEnvironmentVariable("BUILD_NUMBER") ?? "0";
                return $"{branch.Substring(0, Math.Min(branch.Length, 10))}-{build}";
            }
        }
    }
}
