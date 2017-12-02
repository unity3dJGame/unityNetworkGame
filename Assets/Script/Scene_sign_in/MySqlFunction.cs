using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using System.Data;

public class MySqlFunction {
	private static string _str_table_name = "user_info";
	private static string _str_field_user_id = "user_id";
	private static string _str_field_user_code = "user_code";
	private static string _str_operation_equal = "=";
	private static string _str_operation_less = "<";
	private static string _str_operation_less_equal = "<=";
	private static string _str_operation_more = ">";
	private static string _str_operation_more_equal = ">=";

	/// <summary>
	/// Queries the user identifier and code matched.
	/// </summary>
	/// <returns><c>true</c>, if user identifier and code matched was queryed, <c>false</c> otherwise.</returns>
	/// <param name="db">Db.</param>
	/// <param name="id">Identifier.</param>
	/// <param name="code">Code.</param>
	public static bool QueryUserIdAndCodeMatched(MySqlAccess db, string id, string code)
	{
		if (db == null)
			throw new Exception ("db is null.");

		string[] items 		= 	new string[] {_str_field_user_id, _str_field_user_code };
		string[] where_cols =	new string[] {_str_field_user_id,_str_field_user_code };
		string[] operation 	= 	new string[] {_str_operation_equal,	_str_operation_equal };
		string[] values 	= 	new string[] {id, code};

		DataSet result = db.Select (_str_table_name, items, where_cols, operation, values);
		if (result == null || result.Tables.Count < 0)
			return false;
		if (result.Tables[0].Rows.Count == 1) {
			return true;
		} else {
			return false;
		}
	}
}
