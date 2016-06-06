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

		public Client()
		{
		}

		public Client(DataRow dr)
		{
			this.dr = dr;
			Name = dr["Nom_Cli"] as string;
			Surname = dr["Prenom_Cli"] as string;
			Solde = (float)dr["Solde_Cli"];
			Points = (int)dr["PointsFidel_Cli"];
		}

		public override string ToString()
		{
			return "" + Name + " "  + Surname;
		}
	}
}