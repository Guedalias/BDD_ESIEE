using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjetBDD
{
	class ViewModelTable : ViewModelBase, INotifyPropertyChanged
	{
		public ObservableCollection<string> Tables { get; set; }

		private string _mySelectedItem;
		public string MySelectedItem
		{
			get { return _mySelectedItem; }
			set
			{
				_mySelectedItem = value;
				OnPropertyChanged("MySelectedItem");
			}
		}
		private DataTable _myDataTable;

		public event PropertyChangedEventHandler PropertyChanged;

		public DataTable MyDataTable
		{
			get { return _myDataTable; }
			set
			{
				_myDataTable = value;
				OnPropertyChanged("MyDataTable");
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

		public RelayCommand<object> table_SelectionChangedCMD { get; private set; }

		public ViewModelTable()
		{
			table_SelectionChangedCMD = new RelayCommand<object>(table_SelectionChanged);
			Tables = new ObservableCollection<string>();
			Tables.Add("client");
			Tables.Add("commande");
			Tables.Add("composer");
			Tables.Add("ingredient");
			Tables.Add("livreur");
			Tables.Add("pizza");
			Tables.Add("taillepizza");
			Tables.Add("typevehicule");
			Tables.Add("vehicule");
		}

		public override void activate()
		{

		}

		private void table_SelectionChanged(object param)
		{
			string value = param as string;
			SqlConnector sql = SqlConnector.Instance();

			if (sql.IsConnected)
			{
				DataTable dt;
				sql.Select("* FROM " + _mySelectedItem, out dt);
				MyDataTable = dt;
			}
		}
	}
}
