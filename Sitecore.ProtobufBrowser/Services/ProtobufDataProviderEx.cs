using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Abstractions;
using Sitecore.Data.DataProviders.ReadOnly.Protobuf;

namespace Sitecore.ProtobufBrowser.Services
{
    public class ProtobufDataProviderEx : ProtobufDataProvider
    {
        [Obsolete("Obsolete")]
        public ProtobufDataProviderEx(IEnumerable<string> filePaths, BaseLog logger) : base(filePaths, logger)
        {
            File = filePaths?.First();
        }

        public ProtobufDataProviderEx(IEnumerable<string> filePaths, IResourceLoader resourceLoader) : base(filePaths,
            resourceLoader)
        {
            File = filePaths?.First();
        }

        public string File { get; set; }
    }
}