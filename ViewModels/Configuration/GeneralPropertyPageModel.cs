using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class GeneralPropertyPageModel: BasePropertyPageModel
    {
        public YesNoPropertyModel Daemonize { get; set; }
        public StringPropertyModel Supervised { get; set; }
        public StringPropertyModel Pidfile { get; set; }
        public StringPropertyModel Loglevel { get; set; }
        public StringPropertyModel Logfile { get; set; }
        public YesNoPropertyModel SyslogEnabled { get; set; }
        public StringPropertyModel SyslogIdent { get; set; }
        public StringPropertyModel SyslogFacility { get; set; }
        public StringPropertyModel Databases { get; set; }
        public YesNoPropertyModel AlwaysShowLogo { get; set; }

        public GeneralPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            Daemonize = MapToYesNo("daemonize");
            Supervised = MapToString("supervised");
            Pidfile = MapToString("pidfile");
            Loglevel = MapToString("loglevel");
            Logfile = MapToString("logfile");
            SyslogEnabled = MapToYesNo("syslog-enabled");
            SyslogIdent = MapToString("syslog-ident");
            SyslogFacility = MapToString("syslog-facility");
            Databases = MapToString("databases");
            AlwaysShowLogo = MapToYesNo("always-show-logo");
        }
    }
}
