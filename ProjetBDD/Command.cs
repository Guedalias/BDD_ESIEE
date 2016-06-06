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

		public Command()
		{
			ID = -1;
			Price = 0.0f;
		}

		public Command(DataRow dr)
		{
			this.dr = dr;
			ID = int.Parse(dr["ID_Com"] as string);
			DateCmd = DateTime.ParseExact(dr["Date_Com"] as string, "yyyy-MM-dd HH:mm:ss",
									   System.Globalization.CultureInfo.InvariantCulture); 
			DateLivr = DateTime.ParseExact(dr["Date_Liv"] as string, "yyyy-MM-dd HH:mm:ss",
									   System.Globalization.CultureInfo.InvariantCulture);
			Price = float.Parse(dr["Prix_Com"] as string);
			Content = dr["Nom_Piz"] + " "+ dr["Taille_Piz"];
			//= dr["Immat_Veh"];
			//= dr["ID_Cli"];
			//= dr[" ID_Liv"];
		}

		public override string ToString()
		{
			return "" + ID;
		}
	}
}