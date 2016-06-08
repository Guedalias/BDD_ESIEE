using System;
using System.Data;

namespace ProjetBDD
{
	public class Command
	{
		public int		ID { get; set; }
		public float	Price { get; set; }
		public string	Content { get; set; }
		public DateTime DateCmd { get; set; }
		public DateTime DateLivr { get; set; }
		private DataRow dr;

		public RelayCommand<object> ApplyChanges { get; private set; }

		public Command()
		{
			ID = -1;
			Price = 0.0f;

			ApplyChanges = new RelayCommand<object>(ApplyChanges_CB);
		}

		public Command(DataRow dr)
		{
			this.dr = dr;
			ID = (int)(dr["ID_Com"]);
			DateCmd = DateTime.ParseExact(dr["Date_Com"] as string, "yyyy-MM-dd HH:mm:ss",
									   System.Globalization.CultureInfo.InvariantCulture); 
			DateLivr = DateTime.ParseExact(dr["Date_Liv"] as string, "yyyy-MM-dd HH:mm:ss",
									   System.Globalization.CultureInfo.InvariantCulture);
			Price = float.Parse(dr["Prix_Com"] as string);
			Content = dr["Nom_Piz"] + " "+ dr["Taille_Piz"];
			//= dr["Immat_Veh"];
			//= dr["ID_Cli"];
			//= dr[" ID_Liv"];

			ApplyChanges = new RelayCommand<object>(ApplyChanges_CB);
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
		}
	}
}