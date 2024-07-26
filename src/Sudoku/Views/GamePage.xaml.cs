using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Text.Json;
using CoreData;

namespace Sudoku;

[QueryProperty(nameof(ContinueGame), "ContinueGame")]
public partial class GamePage : ContentPage
{
    public bool ContinueGame { get; set; } = false;


	public GamePage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		BindingContext = new GameViewModel(ContinueGame);
    }
	
	private async void Close_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}