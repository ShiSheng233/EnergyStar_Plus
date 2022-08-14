using EnergyStarPlus.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace EnergyStarPlus.Views;

public sealed partial class EnergyStarPage : Page
{
    public EnergyStarViewModel ViewModel
    {
        get;
    }

    public EnergyStarPage()
    {
        ViewModel = App.GetService<EnergyStarViewModel>();
        InitializeComponent();
    }
}
