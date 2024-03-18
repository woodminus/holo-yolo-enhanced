using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * JSONTemplates class
 * for use with Unity
 * Copyright Matt Schoen 2010
 */

public static partial class JSONTemplates {
	static readonly HashSet<object> touched = new HashSet<object>();

	public static JSONObject TOJSON(object obj) {		//For a generic guess
		if(touched.Add(obj)) {
			JSONObject result = JSONObject.obj;
			//Fields
			FieldInfo[] fieldinfo = obj.GetType().GetFields();
			foreach(FieldInfo fi in fieldinfo) {
				JSONObject val = JSONObject.nullJO;
				if(!fi.GetValue(obj).Equals(null)) {
					MethodInfo info = typeof(JSONTemplates).GetMethod("From" + fi.FieldType.Name);
					if(info != null) {
						object[] parms = new object[1];
						parms[0] = fi.GetValue(obj);
						val = (JSONObject)info.Invoke(null, parms);
					} else if(fi.FieldType == typeof(string))
						val = JSONObject.CreateStringObject(fi.GetValue(obj).T