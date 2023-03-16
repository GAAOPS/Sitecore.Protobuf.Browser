using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.DataProviders.ReadOnly.Protobuf;
using Sitecore.Globalization;
using Sitecore.ProtobufBrowser.Models;
using Sitecore.ProtobufBrowser.Settings;
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

        public BaseItem GetItems(string[] paths, string language)
        {
            var provider = new ProtobufDataProvider(paths, resourceLoader);
            var secondaryProvider = GetSecondaryProviders(paths);
            var rootChildren = provider.GetChildIds(sitecoreRootId);

            var rootItem = new BaseItem
            {
                Name = "sitecore"
            };
            foreach (var childId in rootChildren)
                rootItem.Children.Add(GetItem(childId, provider, language, rootItem, secondaryProvider));

            return rootItem;
        }

        private ProtobufDataProvider GetSecondaryProviders(string[] paths)
        {
            if (paths.Length < 2) return null;

            var secondaryFiles = paths.Where(path =>
                !Databases.MainFiles.Contains(Path.GetFileName(path), StringComparer.OrdinalIgnoreCase));

            return new ProtobufDataProvider(secondaryFiles, resourceLoader);
        }

        private BaseItem GetItem(Guid id, ProtobufDataProvider provider, string language, BaseItem parent,
            ProtobufDataProvider secondaryProvider)
        {
            var baseItem = new BaseItem();
            var itemId = ID.Parse(id);
            var definition = provider.GetItemDefinition(itemId);

            baseItem.Name = definition.Name;
            baseItem.ItemDefinition = definition;
            baseItem.Parent = parent;

            var languages = provider.GetItemLanguages(id);

            var secondary = secondaryProvider?.GetItemDefinition(itemId);

            if (!(secondary is null))
            {
                baseItem.Overwritten = true;
                SetParentFlag(baseItem);
            }

            if (!languages.Contains(language)) return baseItem;

            var fields = provider.GetItemFields(id, new VersionUri(Language.Parse(language), Version.Latest));

            baseItem.AddFields("(Item id)", null, itemId.ToString());
            baseItem.AddFields("(Item Path)", null, GetItemPath(baseItem));
            foreach (var kvp in fields)
            {
                var itemDef = provider.GetItemDefinition(ID.Parse(kvp.Key));
                var value = kvp.Value;
                baseItem.AddFields(kvp.Key, itemDef, value);
            }


            var children = provider.GetChildIds(id);

            foreach (var childId in children)
                baseItem.Children.Add(GetItem(childId, provider, language, baseItem, secondaryProvider));

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

        private string GetItemPath(BaseItem item)
        {
            var pathStack = new Stack<string>();
            var current = item;
            pathStack.Push(current.Name);
            while (!(current.Parent is null))
            {
                current = (BaseItem) current.Parent;
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