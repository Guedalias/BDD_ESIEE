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
	class ViewModelClient : ViewModelBase, INotifyPropertyChanged
	{
		public ObservableCollection<Client> Clients { get; set; }


		public event PropertyChangedEventHandler PropertyChanged;

		public RelayCommand<object> clients_SelectionChangedCMD { get; private set; }
		public RelayCommand<object> newClientCMD { get; private set; }

		private Client _mySelectedItem;
		public Client MySelectedItem
		{
			get { return _mySelectedItem; }
			set
			{
				_mySelectedItem = value;
				OnPropertyChanged("MySelectedItem");
			}
		}
		// Create the OnPropertyChanged method to raise the event
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		public ViewModelClient()
		{
			Clients = new ObservableCollection<Client>();
			clients_SelectionChangedCMD = new RelayCommand<object>(client_SelectionChanged);
			newClientCMD = new RelayCommand<object>(client_newClient);
		}

		public override void activate()
		{
			Clients.Clear();
			SqlConnector sql = SqlConnector.Instance();

			if (sql.IsConnected)
			{
				DataTable dt;
				sql.Select("* FROM client", out dt);
				foreach (DataRow dr in dt.Rows)
				{
					Client client = new Client(dr);
					client.UpdateView += activate;
					Clients.Add(client);
				}
			}
		}


		private void client_SelectionChanged(object param)
		{

		}

		private void client_newClient(object param)
		{
			Client client = new Client();
			client.UpdateView += activate;
			Clients.Add(client);
			MySelectedItem = client;
		}

	}
}
