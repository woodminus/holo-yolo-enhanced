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
				if(