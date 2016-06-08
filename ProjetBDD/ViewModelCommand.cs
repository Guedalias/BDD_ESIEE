using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetBDD
{
	class ViewModelCommand : ViewModelBase, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<Command> Commands { get; set; }
		private Command _mySelectedItem;
		public Command MySelectedItem
		{
			get { return _mySelectedItem; }
			set
			{
				_mySelectedItem = value;
				OnPropertyChanged("MySelectedItem");
			}
		}
		public RelayCommand<object> commands_SelectionChangedCMD { get; private set; }
		public RelayCommand<object> newCommandCMD { get; private set; }

		// Create the OnPropertyChanged method to raise the event
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		public ViewModelCommand()
		{
			Commands = new ObservableCollection<Command>();
			commands_SelectionChangedCMD = new RelayCommand<object>(commands_SelectionChanged);
			newCommandCMD = new RelayCommand<object>(newCommand);
		}

		public override void activate()
		{
			Commands.Clear();
			SqlConnector sql = SqlConnector.Instance();
			
			if (sql.IsConnected)
			{
				DataTable dt;
				sql.Select("* FROM commande", out dt);
				foreach (DataRow dr in dt.Rows)
				{
					Commands.Add(new Command(dr));
				}
			}
		}


		private void commands_SelectionChanged(object param)
		{

		}

		private void newCommand(object param)
		{
			SqlConnector sql = SqlConnector.Instance();
			Command cmd = new Command();
			Commands.Add(cmd);
			MySelectedItem = cmd;
		}

	}
}
