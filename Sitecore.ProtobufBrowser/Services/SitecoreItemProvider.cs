using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.DataProviders.ReadOnly.Protobuf;
using Sitecore.Globalization;
using Sitecore.ProtobufBrowser.Models;
using Version = Sitecore.Data.Version;

namespace Sitecore.ProtobufBrowser.Services
{
    public class SitecoreItemProvider : ISitecoreItemProvider
    {
        private readonly IResourceLoader resourceLoader;
        private readonly Guid sitecoreRootId = Guid.Parse("{11111111-1111-1111-1111-111111111111}");

        public SitecoreItemProvider(IResourceLoader resourceLoader)
        {
            this.resourceLoader = resourceLoader;
        }

        public BaseItem GetItems(string mainFile, string moduleFile, string secondaryFile,
            string language)
        {
            var files = new List<string> { mainFile };

            if (!string.IsNullOrEmpty(moduleFile)) files.Add(moduleFile);

            if (!string.IsNullOrEmpty(secondaryFile)) files.Add(secondaryFile);

            var compositeProvider = new ProtobufDataProviderEx(files, resourceLoader);
            var mainProvider = new ProtobufDataProviderEx(new[] { mainFile }, resourceLoader);
            var moduleProvider = string.IsNullOrEmpty(moduleFile)
                ? null
                : new ProtobufDataProviderEx(new[] { moduleFile }, resourceLoader);
            var secondaryProvider = string.IsNullOrEmpty(secondaryFile)
                ? null
                : new ProtobufDataProviderEx(new[] { secondaryFile }, resourceLoader);

            var rootChildren = compositeProvider.GetChildIds(sitecoreRootId);
            var rootItem = new BaseItem
            {
                Name = "sitecore"
            };

            foreach (var childId in rootChildren)
                rootItem.Children.Add(GetItem(childId, compositeProvider, language, rootItem, mainProvider,
                    moduleProvider,
                    secondaryProvider));

            return rootItem;
        }


        private BaseItem GetItem(Guid id, ProtobufDataProviderEx compositeProvider, string language, BaseItem parent,
            ProtobufDataProviderEx mainProvider,
            ProtobufDataProviderEx moduleProvider,
            ProtobufDataProviderEx secondaryProvider)
        {
            var baseItem = new BaseItem();
            var itemId = ID.Parse(id);
            var definition = compositeProvider.GetItemDefinition(itemId);

            baseItem.Name = definition.Name;

            if (baseItem.Name == "Devices")
            {

            }

            baseItem.ItemDefinition = definition;
            baseItem.Parent = parent;
            baseItem.Locations = new List<string>();
            var languages = compositeProvider.GetItemLanguages(id);

            var main = mainProvider.GetItemDefinition(itemId);
            if (!(main is null))
            {
                baseItem.IsInMain = true;
                baseItem.Locations.Add(mainProvider.File);
            }

            var module = moduleProvider?.GetItemDefinition(itemId);


            if (!(module is null))
            {
                baseItem.Overwritten = baseItem.IsInMain;
                baseItem.IsInModule = true;
                baseItem.Locations.Add(moduleProvider.File);
                SetParentModuleFlag(baseItem);
            }


            var secondary = secondaryProvider?.GetItemDefinition(itemId);

            if (!(secondary is null))
            {
                baseItem.Overwritten = baseItem.IsInMain || baseItem.IsInModule;
                baseItem.IsInSecondary = true;
                baseItem.Locations.Add(secondaryProvider.File);
                SetParentFlag(baseItem);
            }

            if (!languages.Contains(language)) return baseItem;

            var fields = compositeProvider.GetItemFields(id, new VersionUri(Language.Parse(language), Version.Latest));

            baseItem.AddFields("(Item id)", null, itemId.ToString());
            baseItem.AddFields("(Item Path)", null, GetItemPath(baseItem));
            foreach (var kvp in fields)
            {
                var itemDef = compositeProvider.GetItemDefinition(ID.Parse(kvp.Key));
                var value = kvp.Value;
                baseItem.AddFields(kvp.Key, itemDef, value);
            }


            var children = compositeProvider.GetChildIds(id);

            foreach (var childId in children)
                baseItem.Children.Add(GetItem(childId, compositeProvider, language, baseItem,
                    mainProvider,
                    moduleProvider,
                    secondaryProvider));

            return baseItem;
        }

        private void SetParentFlag(BaseItem baseItem)
        {
            var parent = baseItem.Parent as BaseItem;

            if (!(parent is null))
            {
                parent.HasSecondaryItems = true;
                SetParentFlag(parent);
            }
        }

        private void SetParentModuleFlag(BaseItem baseItem)
        {
            var parent = baseItem.Parent as BaseItem;

            if (!(parent is null))
            {
                parent.HasModuleItems = true;
                SetParentModuleFlag(parent);
            }
        }

        private string GetItemPath(BaseItem item)
        {
            var pathStack = new Stack<string>();
            var current = item;
            pathStack.Push(current.Name);
            while (!(current.Parent is null))
            {
                current = (BaseItem)current.Parent;
                pathStack.Push(current.Name);
            }

            var path = "";
            while (pathStack.Count > 0)
                path = $"{path}/{pathStack.Pop()}";
            // do some bad things on c#

            return path;
        }
    }
}