<!-- SPDX-License-Identifier: GPL-2.0-or-later -->
<!-- HexChat (Avalonia Port) - .agents/templates/viewmodel-view-template.md -->
<!-- Description: Boilerplate-Vorlage für neue Avalonia Views und CommunityToolkit MVVM ViewModels. -->

# Template: Avalonia View & ViewModel

Nutze diese Vorlage, wenn du eine neue View oder ein neues Dialogfenster in `HexChat.UI` anlegst.

---

## 1. ViewModel (`src/HexChat.UI/ViewModels/ExampleViewModel.cs`)

```csharp
namespace HexChat.UI.ViewModels;

using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class ExampleViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = "HexChat Dialog";

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _inputText = string.Empty;

    public ExampleViewModel()
    {
    }

    [RelayCommand]
    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            // Ausführung der Logik
            await Task.Delay(100, cancellationToken);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        // Dialog schließen / abbrechen
    }
}
```

---

## 2. Avalonia View (`src/HexChat.UI/Views/ExampleView.axaml`)

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:vm="using:HexChat.UI.ViewModels"
             mc:Ignorable="d" d:DesignWidth="450" d:DesignHeight="300"
             x:Class="HexChat.UI.Views.ExampleView"
             x:DataType="vm:ExampleViewModel">

    <Design.DataContext>
        <vm:ExampleViewModel />
    </Design.DataContext>

    <Grid RowDefinitions="Auto,*,Auto" Margin="16">
        <!-- Header -->
        <TextBlock Grid.Row="0"
                   Text="{Binding Title}"
                   FontSize="18"
                   FontWeight="SemiBold"
                   Margin="0,0,0,16" />

        <!-- Content -->
        <StackPanel Grid.Row="1" Spacing="8">
            <TextBlock Text="Eingabe:" />
            <TextBox Text="{Binding InputText}"
                     Watermark="Hier Text eingeben..." />
        </StackPanel>

        <!-- Actions -->
        <StackPanel Grid.Row="2"
                    Orientation="Horizontal"
                    HorizontalAlignment="Right"
                    Spacing="8"
                    Margin="0,16,0,0">
            <Button Content="Abbrechen"
                    Command="{Binding CancelCommand}" />
            <Button Content="Speichern"
                    Classes="accent"
                    Command="{Binding SaveCommand}" />
        </StackPanel>
    </Grid>
</UserControl>
```

---

## 3. Code-Behind (`src/HexChat.UI/Views/ExampleView.axaml.cs`)

```csharp
namespace HexChat.UI.Views;

using Avalonia.Controls;

public partial class ExampleView : UserControl
{
    public ExampleView()
    {
        InitializeComponent();
    }
}
```
