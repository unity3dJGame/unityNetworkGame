using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using MySql.Data.MySqlClient;

namespace Assets
{
	/// <summary>
	/// My sql access.
	/// </summary>
	public class MySqlAccess
	{
		private static MySqlConnection _mysql_connection = null;
		private static string _str_database_name = "mysql";
		private static string _str_ip = "127.0.0.1";
		private static string _str_id = "root";
		private static string _str_pwd = "684268";

		public MySqlAccess()
		{
			OpenMySql();
		}
		/// <summary>
		/// Opens my sql.
		/// </summary>
		public bool OpenMySql() 
		{

			string strConnectString = 
				string.Format ("Database={0};Data Source={1};User Id={2};Password={3};",
					_str_database_name, _str_ip, _str_id, _str_pwd, "3306");  

			try {
				_mysql_connection = new MySqlConnection (strConnectString);
				_mysql_connection.Open ();
			} catch (Exception) {
				Debug.Log ("Open mysql database Exception.");
				return false;
			}

			if (_mysql_connection.State == System.Data.ConnectionState.Open) {
				Debug.Log ("Open mysql success.");
				return true;
			}
			Debug.Log ("Open mysql database failed.");
			return false;
		}

		/// <summary>  
		/// 查询  
		/// </summary>  
		/// <param name="tableName">表名</param>  
		/// <param name="items">需要查询的列</param>  
		/// <param name="whereColName">查询的条件列</param>  
		/// <param name="operation">条件操作符</param>  
		/// <param name="value">条件的值</param>  
		/// <returns></returns>  
		public DataSet Select(string tableName, string[] items, string[] whereColName, string[] operation, string[] value)  
		{  
			if (whereColName.Length != operation.Length || operation.Length != value.Length)  
			{  
				throw new Exception("输入不正确：" + "col.Length != operation.Length != values.Length");  
			}  
			string query = "SELECT " + items[0];  
			for (int i = 1; i < items.Length; i++)  
			{  
				query += "," + items[i];  
			}  
			query += "  FROM  " + tableName + "  WHERE " + " " + whereColName[0] + operation[0] + " '" + value[0] + "'";  
			for (int i = 1; i < whereColName.Length; i++)  
			{  
				query += " AND " + whereColName[i] + operation[i] + "'" + value[i] + "'";  
			}  
			return QuerySet(query);  
		}  

		/// <summary>  
		///   
		/// 执行Sql语句  
		/// </summary>  
		/// <param name="sqlString">sql语句</param>  
		/// <returns></returns>  
		public static DataSet QuerySet(string sqlString)  
		{  
			if (_mysql_connection.State == System.Data.ConnectionState.Open)  
			{  
				DataSet ds = new DataSet();  
				try  
				{  
					MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlString,_mysql_connection);  
					mySqlDataAdapter.Fill(ds);  
				}  
				catch (Exception e)  
				{  
					throw new Exception("SQL:" + sqlString + "/n" + e.Message.ToString());  
				}  
				finally  
				{  

				}  
				return ds;  
			}  
			return null;  
		}  
	}

}