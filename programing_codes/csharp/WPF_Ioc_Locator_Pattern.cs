## App.xaml

<Application xmlns:local="clr-namespace:MyProject">

<Application.Resources>
    <ResourceDictionary>
        <converters:StatusToColorConverter x:Key="StatusToColorConverter"/>
        <local:ViewModelLocator x:Key="Locator"/>
    </ResourceDictionary>
<Application.Resources>

## ViewModelLocator.cs

namespace MyProject;
public class ViewModelLocator
{
    public MainWindowViewModel MainWindowVm => 
        Ioc.Default.GetRequiredService<MainWindowViewModel>();
}


## MainWindow.xaml

<Window 
    DataContext="{Binding MainWindowVm, Source={StaticResource Locator}}">
