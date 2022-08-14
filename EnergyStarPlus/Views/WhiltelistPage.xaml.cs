using EnergyStarPlus.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace EnergyStarPlus.Views;

public sealed partial class WhiltelistPage : Page
{
    public WhiltelistViewModel ViewModel
    {
        get;
    }

    public WhiltelistPage()
    {
        ViewModel = App.GetService<WhiltelistViewModel>();
        InitializeComponent();
    }
}
