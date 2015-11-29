﻿using System;
using System.Configuration;

namespace SimpleGitNotifier
{
    class Configuration
    {
        public string RepositoryPath
        {
            get { return ConfigurationManager.AppSettings["RepositoryPath"]; }
        }
        public string GitFullName
        {
            get { return ConfigurationManager.AppSettings["GitFullName"]; }
        }
        public TimeSpan Interval
        {
            get { return TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["Interval"])); }
        }

        public int DayStart
        {
            get { return int.Parse(ConfigurationManager.AppSettings["DayStart"]); }
        }

        public int DayEnd
        {
            get { return int.Parse(ConfigurationManager.AppSettings["DayEnd"]); }
        }

        public bool WorkOnWeekends
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["WorkOnWeekends"]); }
        }
    }
}