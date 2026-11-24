# BIMPlugins.Windows

[![Revit 2019+](https://img.shields.io/badge/Revit-2019%2B-blue.svg)](https://github.com/Samveyil/BIMPlugins.Windows)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](https://github.com/Samveyil/BIMPlugins.Windows)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/Samveyil/BIMPlugins.Windows/blob/master/LICENSE.txt)
[![NuGet](https://img.shields.io/nuget/v/BIMPlugins.Windows.svg)](https://www.nuget.org/packages/BIMPlugins.Windows)

WPF windows, converters and behaviors for Revit add-ins. Styles come from
[BIMPlugins.MaterialDesign](https://github.com/Samveyil/BIMPlugins.MaterialDesign).

## Installation

```xml
<PackageReference Include="BIMPlugins.Windows" Version="2.0.0" />
```

Targets `net48` (Revit 2019–2024) and `net8.0-windows` (Revit 2025). The Revit API is not included.

Revit loads one copy of the assembly per process, so use the same version in all add-ins.

## MessageWindow

```C#
using BIMPlugins.Windows;

MessageWindow.ShowMessage("Sheets copied.", icon: MessageBoxImage.Information);

if (MessageWindow.ShowMessage("Overwrite the comment?", "Clash viewer",
        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
{
    // ...
}
```

Enter and Esc press the default and cancel buttons, Ctrl+C copies the message.

## ReportWindow

```C#
ReportWindow.ShowReport(closedModels, "Closing models");
```

## Revit Server

Reads the server structure from `%AppData%\BIMPlugins\RevitServer.xml`. Returns `null` on cancel.

```C#
using BIMPlugins.Windows.RevitServer;

RSN.StructureFilePath = @"\\server\share\RevitServer.xml"; // optional, another file

IReadOnlyList<RevitServerFile> files = RSN.SelectFiles();   // all versions
IReadOnlyList<RevitServerFile> files23 = RSN.SelectFiles("2023");
RevitServerFile file = RSN.SelectFile("2023");

// file.Name    OLP_R23_NVD11_BASE.rvt
// file.Path    RSN://srv4/Новая дорога 11/00_КООРД/OLP_R23_NVD11_BASE.rvt
// file.Version 2023
```

## Converters

`xmlns:converters="clr-namespace:BIMPlugins.Windows.Converters;assembly=BIMPlugins.Windows"`

| Converter | |
|---|---|
| `EqualsToVisibilityConverter` | Visible if the value equals `ConverterParameter` |
| `NullableToBoolConverter` | `true` if the value is not null |
| `StringToBoolConverter` | `true` if the value equals `ConverterParameter`; back, `true` sets the parameter |
| `EnumToLocalizedStringConverter` | Enum value as localized text |
| `ViewDuplicateOptionToStringConverter` | `ViewDuplicateOption` as text |
| `RasterQualityTypeToStringConverter` | `RasterQualityType` as text |
| `ColorDepthTypeToStringConverter` | `ColorDepthType` as text |
| `RevitColorToSolidColorBrushConverter` | Revit `Color` as a brush |
| `EscapeUnderscoreConverter` | Keeps `_` in Label, Button and CheckBox text, which otherwise take it as an access key: `A_101` shows as `A_101`, not `A101` |
| `FilePathToFileNameConverter` | `C:\Clashes\report.xml` as `report.xml` |
| `NumberToPercentConverter` | `42` as `42%` |
| `ImageOnLoadConverter` | Image from a file path without locking the file |

## Behaviors

Two-way binding of selected items:

```xml
xmlns:behaviors="clr-namespace:BIMPlugins.Windows.Behaviors;assembly=BIMPlugins.Windows"

<DataGrid behaviors:DataGridBehavior.SelectedItems="{Binding SelectedRows}" />
<ListBox SelectionMode="Extended" behaviors:ListBoxBehavior.SelectedItems="{Binding SelectedItems}" />
```

## Validation

`NotEmptyValidationRule` — required field.

## License

This project is [licensed](https://github.com/Samveyil/BIMPlugins.Windows/blob/master/LICENSE.txt) under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](https://github.com/Samveyil/BIMPlugins.Windows/stargazers)!
