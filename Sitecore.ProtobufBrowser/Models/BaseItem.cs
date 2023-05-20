using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sitecore.Data;

namespace Sitecore.ProtobufBrowser.Models
{
    public class BaseItem : Notifier, IParentItem
    {
        private ObservableCollection<BaseItem> _children = new ObservableCollection<BaseItem>();
        private string _name;
        private IParentItem _parent;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public IParentItem Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                OnPropertyChanged(nameof(Parent));
            }
        }

        public ObservableCollection<BaseItem> Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged(nameof(Children));
            }
        }

        public ItemDefinition ItemDefinition { get; set; }

        public IList<BaseItem> Fields { get; } = new List<BaseItem>();

        public string Value { get; set; }

        /// <summary>
        ///     If user choose two files (primary and secondary) indicates that item is exists in secondary file
        /// </summary>
        public bool Overwritten { get; set; }

        /// <summary>
        ///     If user choose two files (primary and secondary) indicates that
        ///     item children/descendents have and item from secondary file
        /// </summary>
        public bool HasSecondaryItems { get; set; }
        public bool HasModuleItems { get; set; }

        /// <summary>
        /// Based on this property the items from main file can be hidden
        /// </summary>
        public bool NotInMain => Overwritten || HasSecondaryItems || HasModuleItems || IsInModule || IsInSecondary;

        /// <summary>
        /// Based on this property the items from main file can be hidden
        /// </summary>
        public bool NotInModule => (!IsInModule || Overwritten || HasSecondaryItems);

        public bool IsInMain { get; set; }
        public bool IsInModule { get; set; }
        public bool IsInSecondary { get; set; }
        public IList<string> Locations { get; set; }

        public void AddFields(Guid fieldId, ItemDefinition itemDef, string value)
        {
            Fields.Add(new BaseItem
            {
                Name = itemDef is null ? fieldId.ToString() : itemDef.Name,
                ItemDefinition = itemDef,
                Value = value
            });
        }

        public void AddFields(string name, ItemDefinition itemDef, string value)
        {
            Fields.Add(new BaseItem
            {
                Name = name,
                ItemDefinition = itemDef,
                Value = value
            });
        }
    }
}