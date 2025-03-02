namespace Sudoku;

public partial class MenuPage : ContentPage
{
	public MenuPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		string cacheDir = FileSystem.Current.CacheDirectory;
		ContinueBtn.IsEnabled = File.Exists(Path.Combine(cacheDir, "sudoku.json"));
	}

	private async void NewGame_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("Game?ContinueGame=false");
	}
	
	private async void Continue_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("Game?ContinueGame=true");
	}
	
	private async void Settings_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("Settings");
	}
}