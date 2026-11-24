# BIMPlugins.Windows

[![Revit 2019-2025](https://img.shields.io/badge/Revit-2019--2025-blue.svg)](https://github.com/Samveyil/BIMPlugins.Windows)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](https://github.com/Samveyil/BIMPlugins.Windows)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/Samveyil/BIMPlugins.Windows/blob/master/LICENSE.txt)
[![NuGet](https://img.shields.io/nuget/v/BIMPlugins.Windows.svg)](https://www.nuget.org/packages/BIMPlugins.Windows)

WPF windows, converters and behaviors for Revit add-ins in the BIMPlugins look: a message box and a report window,
a Revit Server project picker, value converters, selection behaviors and a validation rule.
The windows use [BIMPlugins.MaterialDesign](https://github.com/Samveyil/BIMPlugins.MaterialDesign), so their styles
never clash with another add-in's MaterialDesign. Texts are in English and Russian, by the language of the UI.

## Installation

One package serves every Revit version:

| Revit       | Target framework  |
|-------------|-------------------|
| 2019 – 2024 | `net48`           |
| 2025        | `net8.0-windows`  |

```xml
<PackageReference Include="BIMPlugins.Windows" Version="2.0.0" />
```

The package does not ship `RevitAPI.dll`. Reference the Revit API in your project as usual,
for example through `Revit_All_Main_Versions_API_x64`.

> Revit loads one copy of an assembly per process, and the first add-in to load it gives it to all the others.
> Use the same version of `BIMPlugins.Windows` in all your add-ins, and update them together.

## Message box

Like `MessageBox.Show`, in the BIMPlugins look and above Revit:

```C#
using BIMPlugins.Windows;

MessageWindow.ShowMessage("The sheets are copied.", icon: MessageBoxImage.Information);

if (MessageWindow.ShowMessage("Overwrite the comment?", "Clash viewer",
        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
{
    // ...
}
```

- Buttons: `OK`, `OKCancel`, `YesNo`, `YesNoCancel`. Enter presses OK or Yes, Esc presses Cancel or No.
- Closing the window answers as the cancel button would: `Cancel`, `No` for Yes/No, `OK` for OK alone.
- Ctrl+C copies the message. A long message scrolls.

## Report window

A list of lines with a button to copy them all:

```C#
ReportWindow.ShowReport(closedModels, "Closing models");
```

Each item is shown as its `ToString()`. Ctrl+C copies every line, Esc closes the window.

## Revit Server projects

The user picks projects from the tree of Revit Server, read from `%AppData%\BIMPlugins\Структура RevitServer.xml`.
A missing or broken file is told to the user, and the result is then `null`, as on cancel.

```C#
using BIMPlugins.Windows.RevitServer;

IReadOnlyList<RevitServerFile> files = RSN.SelectFiles();        // every Revit version
IReadOnlyList<RevitServerFile> files23 = RSN.SelectFiles("2023"); // one version
RevitServerFile file = RSN.SelectFile("2023");                    // one project

foreach (var f in files ?? [])
    Debug.WriteLine($"{f.Name} | {f.Path} | Revit {f.Version}");
// OLP_R23_NVD11_BASE.rvt | RSN://srv4/Новая дорога 11/00_КООРД/OLP_R23_NVD11_BASE.rvt | Revit 2023
```

The filter takes a regular expression and ignores case; text that is not a valid expression is searched for as it is.

## Converters

`xmlns:converters="clr-namespace:BIMPlugins.Windows.Converters;assembly=BIMPlugins.Windows"`

| Converter | Does |
|---|---|
| `EqualsToVisibilityConverter` | Visible when the value equals `ConverterParameter`, compared as text ignoring case: numbers, enums, bools. `TrueValue` / `FalseValue` set the visibilities. |
| `NullableToBoolConverter` | True when the value is not null: enable a part of a window once something is chosen. |
| `StringToBoolConverter` | Radio buttons bound to one property: checked when the property equals the parameter, checking writes the parameter. |
| `EnumToLocalizedStringConverter` | An enum value as its text from the package's resources (`EnumType_Value`), or its name. |
| `ViewDuplicateOptionToStringConverter`, `RasterQualityTypeToStringConverter`, `ColorDepthTypeToStringConverter` | Revit enums as text, in English or Russian, and back. |
| `RevitColorToSolidColorBrushConverter` | A Revit `Color` as a brush; an invalid colour is transparent. |
| `EscapeUnderscoreConverter` | Doubles underscores, so that a Label or CheckBox shows `A_101` as it is. |
| `FilePathToFileNameConverter` | `C:\Clashes\report.xml` as `report.xml`. |
| `NumberToPercentConverter` | `42` as `42%`; anything else as `0%`. |
| `ImageOnLoadConverter` | An image from a file path, read at once so that the file stays free; a missing file shows nothing. |

```xml
<StackPanel Visibility="{Binding ModeIndex, Converter={StaticResource EqualsToVisibility}, ConverterParameter=2}" />
```

## Selected items

`DataGrid.SelectedItems` and `ListBox.SelectedItems` cannot be bound. These attached properties keep a list of the view model
and the control's selection the same, both ways:

```xml
xmlns:behaviors="clr-namespace:BIMPlugins.Windows.Behaviors;assembly=BIMPlugins.Windows"

<DataGrid behaviors:DataGridBehavior.SelectedItems="{Binding SelectedRows}" />
<ListBox SelectionMode="Extended" behaviors:ListBoxBehavior.SelectedItems="{Binding SelectedItems}" />
```

The list must be one that can be changed, such as an `ObservableCollection`.

## Validation

`NotEmptyValidationRule` fails on an empty value with "Required field." in the language of the UI.

## Changelog

See [CHANGELOG.md](https://github.com/Samveyil/BIMPlugins.Windows/blob/master/CHANGELOG.md).

## License

This project is [licensed](https://github.com/Samveyil/BIMPlugins.Windows/blob/master/LICENSE.txt) under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).
