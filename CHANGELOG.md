# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.0.0] / 2026-11-25
### Breaking changes
- `MessageDialog.Show(message, title, buttons, icon, defaultResult)` replaces `MessageWindow.ShowMessage`, as `TaskDialog.Show`
  of the Revit API: a static class with one method, and the window is no longer public. It takes `MessageButtons`,
  `MessageIcon` and returns `MessageResult` instead of `MessageBoxButton`, `MessageBoxImage` and `MessageBoxResult`.
- `ReportDialog.Show(items, title)` replaces `ReportWindow.ShowReport`, and the window is no longer public.
### Features
- `MessageButtons.Close`, the default, and `MessageButtons.RetryCancel`.
- `defaultResult` sets the button that Enter presses and the focus starts on.
### Changed
- `MessageDialog` and `ReportDialog` look closer to the task dialogs of Revit: text in the secondary colour, smaller
  margins, flat buttons, and a thicker frame on the button Enter presses.
- `ReportDialog` has no frame around the list: the lines fill the window from the title bar to the buttons.

## [2.0.2] / 2026-11-25
### Changed
- `MessageWindow`: the buttons stand 20 px apart, and a short message no longer makes the window wider than 300 px.

## [2.0.1] / 2026-11-24
### Changed
- `BIMPlugins.MaterialDesign` 5.3.2.2: narrow scroll bars, as in Visual Studio.
- `MessageWindow`: more room around the message, a larger icon, and the buttons on a gray strip, as in task dialogs.
- `ReportWindow`: the buttons on a gray strip, smaller margins, and long lines wrap instead of being cut off.
### Fixed
- Scroll bars no longer cover the rounded corners of the frames around lists, and the frames in the Revit Server
  windows stand as far from the right edge as from the left.
- The Russian texts are shown for every Russian culture, such as `ru-KZ` and `ru-BY`, not only `ru-RU`:
  the translation is in the `ru` folder instead of `ru-RU`.

## [2.0.0] / 2026-11-24
First release on nuget.org.
### Breaking changes
- The package uses `BIMPlugins.MaterialDesign` instead of `MaterialDesignThemes`, and the windows take the
  `BIMPluginsTheme` colours.
- `MessageWindow.ShowMessage(message, title, buttons, icon)` takes a `MessageBoxButton` instead of `bool useOKButton`:
  `ShowMessage(text, MessageBoxImage.Question, false)` becomes
  `ShowMessage(text, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question)`.
  The window's constructor and `MessageViewModel` are no longer public.
- `ReportWindow.ShowReport(items, title)` replaces `new ReportWindow(items).ShowDialog()`; the constructor is no longer public.
- `RSN.SelectFiles(version)` and `RSN.SelectFile(version)` return `RevitServerFile` (name, path, Revit version) and replace
  `GetFilePaths` and `GetFilePath`. The Revit Server windows, view models, `FileItem` and `FolderItem` are no longer public.
- `EqualsToVisibilityConverter` replaces `IntToVisibilityConverter`: it compares any value with the parameter as text.
- `ViewDuplicateOptionToStringConverter`, `RasterQualityTypeToStringConverter` and `ColorDepthTypeToStringConverter`
  show the text in the language of the UI, English or Russian. Back they still take the Russian texts.
- Windows are owned by the active Revit window instead of being `Topmost` above every program.
- The Revit Server structure file is `%AppData%\BIMPlugins\RevitServer.xml` instead of `Структура RevitServer.xml`.
### Features
- `RSN.StructureFilePath` sets another structure file.
- `MessageWindow`: OK/Cancel and Yes/No/Cancel buttons, a title, Enter and Esc, Ctrl+C to copy the message,
  scrolling for a long message.
- `ReportWindow`: a title, a button and Ctrl+C to copy every line, Esc to close.
- `EnumToLocalizedStringConverter` for any enum with texts in the package's resources.
- `StringToBoolConverter` converts back: `true` writes `ConverterParameter` to the source.
- English and Russian texts for the windows, converters and `NotEmptyValidationRule`.
- XML documentation for the whole public API, README and icon in the package, symbol package (`.snupkg`).
- Publish to nuget.org from GitHub Actions with Trusted Publishing.
### Fixed
- The Revit Server filter no longer fails on text that is not a valid regular expression, such as `(` or `[`,
  and ignores case.
- A missing or broken Revit Server structure file, or a missing Revit version in it, is told to the user instead of
  failing the add-in.
- `DataGridBehavior` and `ListBoxBehavior` no longer add a handler on every new list, fail with
  "Collection was modified" or on a null list, and work with single selection.
- `NumberToPercentConverter` no longer fails on text that is not a number.
- `ImageOnLoadConverter` no longer fails on a relative path or a missing, locked or broken file.
- `RevitColorToSolidColorBrushConverter` no longer fails on an invalid Revit colour.
- Copying a Revit Server path no longer fails when another program holds the clipboard.
### Removed
- The dependency on `CommunityToolkit.Mvvm`: the package depends on `BIMPlugins.MaterialDesign` alone.
- The debug test command of the Revit Server picker.

[3.0.0]: https://github.com/Samveyil/BIMPlugins.Windows/tree/v3.0.0
[2.0.2]: https://github.com/Samveyil/BIMPlugins.Windows/tree/v2.0.2
[2.0.1]: https://github.com/Samveyil/BIMPlugins.Windows/tree/v2.0.1
[2.0.0]: https://github.com/Samveyil/BIMPlugins.Windows/tree/v2.0.0
