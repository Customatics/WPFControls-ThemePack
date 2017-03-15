# WPFControls-ThemePack
Custom designed themes for WPF controls to make your app look better. Simple to modify.

## Intro
This theme is made to have an individually styled controls set for WPF apps, which depends on few configurable properties (like colors and sizes), so it can be easily modified according to design requirements.

**Current state:** this version contains most commonly used controls. More controls are coming.

**Further plans on theme development and releases:** update incomplete templates, add a few more configurable values such as corner radius for controls, animations, etc.

## Special features
* There is a Numerics.xaml, which contains most of the pre-defined values that are used across the theme such as paddings, margins, font-sizes, opacities and border widths.
* 2 color schemes consisting of 12 colors each, which means a small change has an influence on the whole theme style.
* Re-worked Window template that scales correctly on different resolutions (including retina), multimonitor systems and even DPI’s.
* Re-constructed datepicker including popup content.
* Attached property that allows to use glyph fonts instead of usual path data, so now you can apply icons to the controls in a much simpler way, keeping the UI consistent at the same time.

## UI Modifications
* Specially updated scrollbar template, which is thin by default, but expands when hovered. 
* Tab control may wrap Tab items or scroll them.

## Screenshots
### Light color scheme
![alt text](http://customatics.com/wp-content/uploads/2017/03/light-colors.png "Colors for light color scheme")
![alt text](http://customatics.com/wp-content/uploads/2017/03/light-texts.png "Text sizes")
![alt text](http://customatics.com/wp-content/uploads/2017/03/light-buttons.png "Button styles")
![alt text](http://customatics.com/wp-content/uploads/2017/03/light-forms.png "Form elements styles")
![alt text](http://customatics.com/wp-content/uploads/2017/03/light-scrolls.png "Scrollbars and scrollviewer styles")
![alt text](http://customatics.com/wp-content/uploads/2017/03/light-progress.png "Progressbar style")
![alt text](http://customatics.com/wp-content/uploads/2017/03/light-notifications.png "Notifications styles")
![alt text](http://customatics.com/wp-content/uploads/2017/03/light-context.png "Context menu style")

### Dark color scheme
![alt text](http://customatics.com/wp-content/uploads/2017/03/dark-colors.png "Colors for light color scheme")
![alt text](http://customatics.com/wp-content/uploads/2017/03/dark-texts.png "Text sizes")
![alt text](http://customatics.com/wp-content/uploads/2017/03/dark-buttons.png "Button styles")
![alt text](http://customatics.com/wp-content/uploads/2017/03/dark-forms.png "Form elements styles")
![alt text](http://customatics.com/wp-content/uploads/2017/03/dark-scrolls.png "Scrollbars and scrollviewer styles")
![alt text](http://customatics.com/wp-content/uploads/2017/03/dark-progress.png "Progressbar style")
![alt text](http://customatics.com/wp-content/uploads/2017/03/dark-notifications.png "Notifications styles")
![alt text](http://customatics.com/wp-content/uploads/2017/03/dark-context.png "Context menu style")

## Documentation
**Note:** in some cases you may need to apply styles directly (i.e. buttons, texts, windows).

### Applying theme to your project controls
```
<Application x:Class="ThemeWindow.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/Theme_001;component/Themes/Values/Numerics.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/ColorSchemes/Light.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Icons/Icons.xaml"/>

                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/Buttons.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/ContextMenu.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/DatePicker.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/Form.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/Menu.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/Notifications.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/Options.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/ProgressBar.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/Scroll.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/Slider.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/TabControl.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/Texts.xaml"/>
                <ResourceDictionary Source="/Theme_001;component/Themes/Styles/Window.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```
### Applying theme to your window
```
<base:BaseWindow x:Class="ThemeWindow.Windows.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:base="clr-namespace:ThemePack.Common.Base;assembly=ThemePack.Common"
        xmlns:c="clr-namespace:ThemePack.Common.AttachedProperties;assembly=ThemePack.Common"
        xmlns:sys="clr-namespace:System;assembly=mscorlib"
        mc:Ignorable="d" 
        Closing="Window_Closing"
        Title="Theme preview" Width="900" Height="600" Style="{StaticResource Window.ModalStyle.NormalStyle}" SizeToContent="Manual">
</base:BaseWindow>
```
