using System.Collections.Generic;

namespace Sitecore.ProtobufBrowser.Settings
{
    public static class Databases
    {
        public const string MasterFile = "items.master.dat";
        public const string CoreFile = "items.core.dat";
        public const string WebFile = "items.web.dat";

        public static IList<string> MainFiles { get; } = new List<string> {CoreFile, MasterFile, WebFile};
    }
}