## DiExtension.cs

namespace MyProject;
public class DiExtension : MarkupExtension
{
    public Type? Type { get; set; }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (Type == null)
        { return null; }

        if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            return Ioc.Default.GetService(Type) ??
                throw new InvalidOperationException($"No service of type {Type} registered.");
        }

        // 디자인 Mode 일 경우 기본 생성
        try
        {
            return Activator.CreateInstance(Type);
        }
        catch
        {
            return null;
        }
    }
}

## SomeViewModel

public class SomeViewModel : ObservableObject
{
    private string _item;
    public string Item {
        get => _item; set => SetProperty(ref _item, value);
    }
}

## SomeView

<UserControl xmlns:root="clr-namespace:MyProject"
            DataContext="{root:Di Type={x:Type vm:SomeViewModel}}">
