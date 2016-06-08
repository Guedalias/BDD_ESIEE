using System.Data;

namespace ProjetBDD
{
	public class Client
	{
		private DataRow dr = null;
		public string	Name { get; set; }
		public string	Surname { get; set; }
		public float	Solde { get; set; }
		public int		Points { get; set; }
		public int		ID { get; private set; }

		public RelayCommand<object> ApplyChanges { get; private set; }
		public delegate void updateView();

		public updateView UpdateView { get; set; }

		public Client()
		{
			ID = -1;
			ApplyChanges = new RelayCommand<object>(ApplyChanges_CB);
		}

		public Client(DataRow dr)
		{
			this.dr = dr;
			ID = (int)dr["ID_Cli"];
			Name = dr["Nom_Cli"] as string;
			Surname = dr["Prenom_Cli"] as string;
			Solde = (float)dr["Solde_Cli"];
			Points = (int)dr["PointsFidel_Cli"];
			ApplyChanges = new RelayCommand<object>(ApplyChanges_CB);
		}

		public object[] toSqlTab()
		{
			object[] res = new object[5];
			res[0] = ID;
			res[1] = "'" + Name + "'";
			res[2] = "'" + Surname + "'";
			res[3] = Solde as object;
			res[4] = Points as object;
			return res;
		}

		public object[] toSqlMap()
		{
			object[] res = new object[4];
			res[0] = "Nom_Cli=" + "'" + Name + "'";
			res[1] = "Prenom_Cli=" + "'" + Surname + "'";
			res[2] = "Solde_Cli=" + Solde as object;
			res[3] = "PointsFidel_Cli=" + Points as object;
			return res;
		}

		public override string ToString()
		{
			return "" + Name + " "  + Surname;
		}

		public void ApplyChanges_CB(object param)
		{
			SqlConnector sql = SqlConnector.Instance();
			if (ID >= 0)
				sql.updateClient(this);
			else
				sql.CreateClient(this);
			//Notify Parent
			UpdateView();
		}
	}
}