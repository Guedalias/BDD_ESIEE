using System;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Collections.ObjectModel;

namespace ProjetBDD
{
	class SqlConnector
	{
		private SqlConnector()
		{
		}

		private string databaseName = string.Empty;
		public	string DatabaseName
		{
			get { return databaseName; }
			set { databaseName = value; }
		}

		private MySqlConnection connection = null;
		public	MySqlConnection Connection	{ get { return connection; }}
		public	string User					{ get; set; }
		public	string Password				{ get; set; }

		private bool _isConnected = false;
		public bool IsConnected
		{
			get { return _isConnected; }
		}

		//Singleton Impl
		private static SqlConnector _instance = null;
		public static SqlConnector Instance()
		{
			if (_instance == null)
				_instance = new SqlConnector();
			return _instance;
		}

		public bool Connect()
		{
			if (Connection == null)
			{
				if (String.IsNullOrEmpty(databaseName))
					return false;
				string connstring = string.Format("Server=localhost; database={0}; UID={1}", databaseName, User); // ; password={2} , Password
				connection = new MySqlConnection(connstring);
				connection.Open();
			}
			if (ConnectionState.Open == connection.State)
			{
				_isConnected = true;
				return true;
			}
			return false;
		}

		public bool ExecQuery(string query)
		{
		    //create command and assign the query and connection from the constructor
			MySqlCommand cmd = new MySqlCommand(query, connection);

			//Execute command
			if (cmd.ExecuteNonQuery() == 0)
				return true;
			return false;
		}

		private string objectTabToString(object[] data)
		{
			string str_data = "";
			foreach (object it in data)
			{
				str_data += it + ",";
			}
			return str_data;
		}

		public void Insert(string target, object[] data, string column)
		{
			string str_data = objectTabToString(data);
			string query = "Insert into " + target + "(" + column + ") values(" + str_data.Substring(0, str_data.Length -1) + ");";
			ExecQuery(query);
		}

		public void Update(string target, int id, string idName, object[] data)
		{
			string str_data = objectTabToString(data);
			string query = "Update " + target + " Set " + str_data.Substring(0, str_data.Length - 1) + " where " + idName + "=" + id + ";";
			ExecQuery(query);
		}

		public void Select(string query, out DataTable dt)
		{
			string				select = "SELECT " + query;
			List<List<string>>	res = new List<List<string>>();

			//Create Command
			MySqlCommand cmd = new MySqlCommand(select, connection);
			//Create a data reader and Execute the command
			MySqlDataReader dataReader = cmd.ExecuteReader();
			dt = new DataTable();
			Load(dt, dataReader, true);
			dataReader.Close();
		}

		public void Load(DataTable table, IDataReader reader, bool createColumns)
		{
			if (createColumns)
			{
				table.Columns.Clear();
				var schemaTable = reader.GetSchemaTable();
				foreach (DataRowView row in schemaTable.DefaultView)
				{
					var columnName = (string)row["ColumnName"];
					var type = (Type)row["DataType"];
					table.Columns.Add(columnName, type);
				}
			}
			table.Load(reader);
		}

		public void CreateClient(Client c)
		{
			Insert("client", c.toSqlTab(), c.getColumnNames());
		}

		public void updateClient(Client c)
		{
			Update("client", c.ID, "ID_Cli", c.toSqlMap());
		}

		public void CreateCommand(Command c)
		{
			Insert("commande", c.toSqlTab(), c.getColumnNames());
		}

		public void updateCommand(Command c)
		{
			Update("commande", c.ID, "ID_Com", c.toSqlMap());
		}

		public ConnectionState GetConnectionState()
		{
			return connection.State;
		}

		public void Close()
		{
			if (connection != null)
			{
				_isConnected = false;
				connection.Close();
				connection = null;
			}
		}
	}
}
