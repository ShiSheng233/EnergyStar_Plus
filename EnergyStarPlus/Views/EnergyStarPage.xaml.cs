using EnergyStar;
using EnergyStarPlus.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace EnergyStarPlus.Views;

public sealed partial class EnergyStarPage : Page
{
    private static Thread? ESService;
    private async void ShowMessageBox(string title, string text)
    {
        ContentDialog cd = new()
        {
            Title = title,
            Content = text,
            CloseButtonText = "OK",
            XamlRoot = this.Content.XamlRoot
        };
        await cd.ShowAsync();
    }
    
    public EnergyStarViewModel ViewModel
    {
        get;
    }

    public EnergyStarPage()
    {
        ViewModel = App.GetService<EnergyStarViewModel>();
        InitializeComponent();

        if (ESService != null && ESService.IsAlive)
        {
            EnergyStarToggle.IsChecked = true;
            EnergyStarStatusText.Text = "On";
        }
        else EnergyStarStatusText.Text = "Off";
    }

    private async void ToggleButton_Checked(object sender, RoutedEventArgs e)
    {   
        if (Environment.OSVersion.Version.Build < 22000)
        {
            ShowMessageBox("Error", "You are running on an unsupported platform.");
            return;
        }

        try
        {
            ESService = new(new ThreadStart(EnergyManager.MainService));
            ESService.Start();
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error", ex.Message);
        }
    }

    private async void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ESService == null || !ESService.IsAlive)
            {
                ShowMessageBox("Error", "EnergyStar is not running.");
                EnergyManager.BoostAllInfluencedProcesses();
            }
            else
            {
                EnergyManager.StopService();
            }
            EnergyStarStatusText.Text = "Off";
        }
        catch (Exception ex)
        {
            ShowMessageBox("Error",ex.Message);
        }
    }
}
