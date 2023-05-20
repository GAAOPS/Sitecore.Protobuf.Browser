using Sitecore.ProtobufBrowser.Models;

namespace Sitecore.ProtobufBrowser.Services
{
    public interface ISitecoreItemProvider
    {
        BaseItem GetItems(string sourceDialogMainFile, string sourceDialogModuleFile, string sourceDialogSecondaryFile, string language);
    }
}