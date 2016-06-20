using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

namespace ProjetBDD
{
	public class Command: INotifyPropertyChanged
	{
		public int		ID { get; set; }
		private float _price;
		public float Price
		{
			get { return _price; }
			set
			{
				_price = value;
				OnPropertyChanged("Price");
			}
		}

		public float BasePrice { get; set; }
		public float PriceModif { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public DateTime DateCmd { get; set; }
		public DateTime DateLivr { get; set; }
		public bool Visible {
			get { return (ID >= 0); }
		}
		public bool ApplyVisible
		{
			get { return !(ID >= 0); }
		}

		public ObservableCollection<string> Pizza { get; set; }
		public string selectedPizza { get; set; }
		public ObservableCollection<string> TaillePizza { get; set; }
		public string selectedTaille { get; set; }
		public ObservableCollection<string> Vehicule { get; set; }
		public string selectedVehicule { get; set; }
		public ObservableCollection<string> Livreur { get; set; }
		public string selectedLivreur { get; set; }
		public ObservableCollection<string> Client { get; set; }
		public string selectedClient { get; set; }
		private DataRow dr;

		public RelayCommand<object> ApplyChanges { get; private set; }

		public RelayCommand<object> CloseCMD { get; private set; }
		public RelayCommand<object> pizzaChangedCMD { get; private set; }

		public delegate void updateView();

		public updateView UpdateView { get; set; }

		public Command()
		{
			DateCmd = DateTime.Now;
			DateLivr = DateTime.Now;
			ID = -1;
			Price = 0.0f;
			init();
		}

		public Command(DataRow dr)
		{
			this.dr = dr;
			ID = (int)(dr["ID_Com"]);
			DateCmd = (DateTime)(dr["Date_Com"]);
			DateLivr = (DateTime)(dr["Date_Liv"]);
			Price = (float)(dr["Prix_Com"]);
			selectedPizza = (dr["Nom_Piz"]) as string;
			selectedTaille = (dr["Taille_Piz"]) as string;
			selectedVehicule = (dr["Immat_Veh"]) as string;
			
			DataTable dt;
			SqlConnector sql = SqlConnector.Instance();

			sql.Select("Nom_Cli from client where ID_Cli='" + dr["ID_Cli"] + "'", out dt);
			selectedClient = dt.Rows[0]["Nom_Cli"] as string;
			sql.Select("Nom_Liv from livreur where ID_Liv='" + dr["ID_Liv"] + "'", out dt);
			selectedLivreur = dt.Rows[0]["Nom_Liv"] as string;
			init();
		}

		private void init()
		{
			ApplyChanges = new RelayCommand<object>(ApplyChanges_CB);

			CloseCMD = new RelayCommand<object>(Close_CB);
			pizzaChangedCMD = new RelayCommand<object>(pizzaChanged_CB);

			SqlConnector sql = SqlConnector.Instance();
			DataTable dt;

			Pizza = new ObservableCollection<string>();
			sql.Select("Nom_Piz from Pizza", out dt);
			foreach (DataRow dr in dt.Rows)
			{
				Pizza.Add(dr["Nom_Piz"] as string);
			}
			TaillePizza = new ObservableCollection<string>();
			sql.Select("Taille_Piz from TaillePizza", out dt);
			foreach (DataRow dr in dt.Rows)
			{
				TaillePizza.Add(dr["Taille_Piz"] as string);
			}
			Vehicule = new ObservableCollection<string>();
			sql.Select("Immat_Veh from vehicule", out dt);
			foreach (DataRow dr in dt.Rows)
			{
				Vehicule.Add(dr["Immat_Veh"] as string);
			}
			Livreur = new ObservableCollection<string>();
			sql.Select("Nom_Liv from livreur", out dt);
			foreach (DataRow dr in dt.Rows)
			{
				Livreur.Add(dr["Nom_Liv"] as string);
			}
			Client = new ObservableCollection<string>();
			sql.Select("Nom_Cli from client", out dt);
			foreach (DataRow dr in dt.Rows)
			{
				Client.Add(dr["Nom_Cli"] as string);
			}
		}

		private void Close_CB(object obj)
		{
			SqlConnector sql = SqlConnector.Instance();
			DataTable dt;
			sql.Select("* from Client where Nom_Cli='" + selectedClient + "'", out dt);
			Client client = new Client(dt.Rows[0]);
			if (client.Points == 10)
			{
				client.Points = 0;
			} else
			{
				client.Solde -= Price;
				client.Points += 1;
			}
			sql.updateClient(client);
		}


		private void pizzaChanged_CB(object obj)
		{
			SqlConnector sql = SqlConnector.Instance();
			DataTable dt;
			sql.Select("PrixBase_Piz from Pizza where Nom_Piz='" + selectedPizza +"'", out dt);
			BasePrice = (float)dt.Rows[0]["PrixBase_Piz"];
			if (selectedTaille == "humaine")
				PriceModif = 1.0f;
			if (selectedTaille == "ogresse")
				PriceModif = 1.3f;
			if (selectedTaille == "naine")
				PriceModif = 0.7f;
			Price = BasePrice * PriceModif;
		}

		public override string ToString()
		{
			return "" + ID;
		}

		public void ApplyChanges_CB(object param)
		{
			SqlConnector sql = SqlConnector.Instance();
			if (ID >= 0)
				sql.updateCommand(this);
			else
				sql.CreateCommand(this);
			//Notify Parent
			UpdateView();
		}

		public object[] toSqlTab()
		{
			SqlConnector sql = SqlConnector.Instance();
			object idLiv, idClient;
			DataTable dt;

			sql.Select("ID_CLI from client where Nom_Cli='" + selectedClient + "'", out dt);
			idClient = dt.Rows[0]["ID_CLI"];
			sql.Select("ID_Liv from livreur where Nom_Liv='" + selectedLivreur + "'", out dt);
			idLiv = dt.Rows[0]["ID_Liv"];
			object[] res = new object[8];
			res[0] = "'" + DateCmd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
			res[1] = "'" + DateLivr.ToString("yyyy-MM-dd HH:mm:ss") + "'";
			res[2] = Price as object;
			res[3] = "'" + selectedTaille + "'" ;
			res[4] = "'" + selectedPizza + "'" ;
			res[5] = "'" + selectedVehicule + "'";
			res[6] = idClient;
			res[7] = idLiv;
			return res;
		}

		public object[] toSqlMap()
		{
			DateLivr = DateTime.Now;
			object[] res = new object[4];
			res[0] = "Date_Com=" + "'" + DateCmd.ToString("yyyy-MM-dd HH:mm:ss") + "'";
			res[1] = "Date_Liv=" + "'" + DateLivr.ToString("yyyy-MM-dd HH:mm:ss") + "'";
			res[2] = "Prix_Com=" + Price;
			res[3] = "Taille_Piz=" + selectedTaille;
			res[3] = "Nom_Piz=" + selectedPizza;
			res[3] = "Immat_Veh=" + selectedVehicule;
			res[3] = "ID_Cli=" + selectedClient;
			res[3] = "ID_Liv=" + selectedLivreur;
			return res;
		}

		public string getColumnNames()
		{
			return "Date_Com, Date_Liv, Prix_Com, Taille_Piz, Nom_Piz, Immat_Veh, ID_Cli, ID_Liv";
		}

		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}