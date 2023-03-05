using Sitecore.ProtobufBrowser.Models;

namespace Sitecore.ProtobufBrowser.Services
{
    public interface ISitecoreItemProvider
    {
        BaseItem GetItems(string[] paths, string language);
    }
}