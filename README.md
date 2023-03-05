# Sitecore Protobuf Browser

Sitecore uses Protobuf to store the original/default contents of databases to the disk in these paths which are defined in the databases configs:

- ./App_Data/items/core/items.core.dat
- ./App_Data/items/master/items.master.dat
- ./App_Data/items/web/items.web.dat

Protobuf is a google based serializer: https://github.com/protocolbuffers/protobuf which is quite fast.

Here I have created a wpf app which allows you to browse this serialized files and check their contents.
You can choose multiple files and items in the secondary files (none O.O.B) will be shown in red in the content tree, which means they are overwritten or are new in the secondary files.

The fields in parentheses are added manually for information purposes  (ex. Item id, item path).


You need to select one of the original files listed above, with/without corresponding secondary file(s).

![gif](./Protobuf.gif)


### Limitations:
- There is no post processing or evaluating the inheritance chain or standard values. It simply reads what is stored in the .dat files.
- There is no version selector (Latest version is used)
- There is no language selector (only English version of items is shown)
- No search capabilities in place

Project is based on .net 4.8 sdk style with xmcloud assemblies are used in project. feel free to change them to sitecore kernel and build it.

### Change log
- Context menu for field section is added to provide easier copy