namespace Sudoku;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// register routes
		Routing.RegisterRoute("Game", typeof(GamePage));

		Routing.RegisterRoute("Settings", typeof(SettingsPage));
	}
}
