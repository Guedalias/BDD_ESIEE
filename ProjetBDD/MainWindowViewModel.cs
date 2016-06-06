using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ProjetBDD
{
	internal class MainWindowViewModel : INotifyPropertyChanged
	{
		public RelayCommand<object> ConnectDB { get; private set; }
		public RelayCommand<object> ChangeView { get; private set; }

		public ViewModelBase _currentView;
		public ViewModelBase CurrentView
		{
			get
			{
				return _currentView;
			}
			set
			{
				if (value != _currentView)
				{
					_currentView = value;
					OnPropertyChanged("CurrentView");
				}
			}
		}

		public List<ViewModelBase> views; 
		private MainWindow _window;

		public event PropertyChangedEventHandler PropertyChanged;

		// Create the OnPropertyChanged method to raise the event
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		public MainWindowViewModel()
		{
			CurrentView = null;
			views = new List<ViewModelBase>();
			views.Add(new ViewModelTable());
			views.Add(new ViewModelCommand());
			views.Add(new ViewModelClient());
		}

		public bool init(MainWindow window)
		{
			_window = window;
			ConnectDB = new RelayCommand<object>(_connect);
			ChangeView = new RelayCommand<object>(_changeView);
			return true;
		}

		public void _connect(object param)
		{
			_window.Logger.Text = "Connecting";
			var dbCon = SqlConnector.Instance();
			dbCon.DatabaseName = "RaPizz";
			dbCon.User = _window.Tb_user.Text;
			dbCon.Password = _window.Tb_pass.Text; ;
			if (dbCon.Connect())
			{
				//suppose col0 and col1 are defined as VARCHAR in the DB
				string query = "SELECT * FROM pizza";
				var cmd = new MySqlCommand(query, dbCon.Connection);
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					string someStringFromColumnZero = reader.GetString(0);
					string someStringFromColumnOne = reader.GetString(1);
					Console.WriteLine(someStringFromColumnZero + "," + someStringFromColumnOne);
				}
				_window.Logger.Text = "Connected as " + _window.Tb_user.Text;
				reader.Close();
			}
		}

		public void _changeView(object param)
		{
			Console.WriteLine("ChangeVIEW");
			int idx = int.Parse(param as string);
			CurrentView = views[idx];
			CurrentView.activate();
		}
	}
}