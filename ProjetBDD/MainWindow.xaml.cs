using System;
using System.Windows;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace ProjetBDD
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MainWindowViewModel context;
		
		public MainWindow()
		{
			this.DataContext = new MainWindowViewModel();
			context = this.DataContext as MainWindowViewModel;
			context.init(this);
			InitializeComponent();
		}

	}
}
