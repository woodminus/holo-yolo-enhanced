#define PRETTY		//Comment out when you no longer need to read JSON to disable pretty Print system-wide
//Using doubles will cause errors in VectorTemplates.cs; Unity speaks floats
#define USEFLOAT	//Use floats for numbers instead of doubles	(enable if you're getting too many significant digits in string output)
//#define POOLING	//Currently using a build setting for this one (also it's experimental)

#if UNITY_2 || UNITY_3 || UNITY_4 || UNITY_5
using UnityEngine;
using Debug = UnityEngine.Debug;
#endif
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * JSONObject class v.1.4.1
 * for use with Unity
 * Copyright Matt Schoen 2010 - 2013
 */

public class JSONObject {
#if POOLING
	const int MAX_POOL_SIZE = 10000;
	public static Queue<JSONObject> releaseQueue = new Queue<JSONObject>();
#endif

	const int MAX_DEPTH = 100;
	const string INFINITY = "\"INFINITY\"";
	const string NEGINFINITY = "\"NEGINFINITY\"";
	const string NaN = "\"NaN\"";
	const string NEWLINE = "\r\n";
	public static readonly char[] WHITESPACE = { ' ', '\r', '\n', '\t', '\uFEFF', '\u0009' };
	public enum Type { NULL, STRING, NUMBER, OBJECT, ARRAY, BOOL, BAKED }
	public bool isContainer { get { return (type == Type.ARRAY || type == Type.OBJECT); } }
	public Type type = Type.NULL;
	public int Count {
		get {
			if(list == null)
				return -1;
			return list.Count;
		}
	}
	public List<JSONObject> list;
	public List<string> keys;
	public string str;
#if USEFLOAT
	public float n;
	public float f {
		get {
			return n;
		}
	}
#else
	public double n;
	public float f {
		get {
			return (float)n;
		}
	}
#endif
	public bool useInt;
	public long i;
	public bool b;
	public delegate void AddJSONContents(JSONObject self);

	public static JSONObject nullJO { get { return Create(Type.NULL); } }	//an empty, null object
	public static JSONObject obj { get { return Create(Type.OBJECT); } }		//an empty object
	public static JSONObject arr { get { return Create(Type.ARRAY); } }		//an empty array

	public JSONObject(Type t) {
		type = t;
		switch(t) {
			case Type.ARRAY:
				list = new List<JSONObject>();
				break;
			case Type.OBJECT:
				list = new List<JSONObject>();
				keys = new List<string>();
				break;
		}
	}
	public JSONObject(bool b) {
		type = Type.BOOL;
		this.b = b;
	}
#if USEFLOAT
	public JSONObject(float f) {
		type = Type.NUMBER;
		n = f;
	}
#else
	public JSONObject(double d) {
		type = Type.NUMBER;
		n = d;
	}
#endif
	public JSONObject(int i) {
		type = Type.NUMBER;
		this.i = i;
		useInt = true;
		n = i;
	}
	public JSONObject(long l) {
		type = Type.NUMBER;
		i = l;
		useInt = true;
		n = l;
	}
	public JSONObject(Dictionary<string, string> dic) {
		type = Type.OBJECT;
		keys = new List<string>();
		list = new List<JSONObject>();
		//Not sure if it's worth removing the foreach here
		foreach(KeyValuePair<string, string> kvp in dic) {
			keys.Add(kvp.Key);
			list.Add(CreateStringObject(kvp.Value));
		}
	}
	public JSONObject(Dictionary<string, JSONObject> dic) {
		type = Type.OBJECT;
		keys = new List<string>();
		list = new List<JSONObject>();
		//Not sure if it's worth removing the foreach here
		foreach(KeyValuePair<string, JSONObject> kvp in dic) {
			keys.Add(kvp.Key);
			list.Add(kvp.Value);
		}
	}
	public JSONObject(AddJSONContents content) {
		content.Invoke(this);
	}
	public JSONObject(JSONObject[] objs) {
		type = Type.ARRAY;
		list = new List<JSONObject>(objs);
	}
	//Convenience function for creating a JSONObject containing a string.  This is not part of the constructor so that malformed JSON data doesn't just turn into a string object
	public static JSONObject StringObject(string val) { return CreateStringObject(val); }
	public void Absorb(JSONObject obj) {
		list.AddRange(obj.list);
		keys.AddRange(obj.keys);
		str = obj.str;
		n = obj.n;
		useInt = obj.useInt;
		i = obj.i;
		b = obj.b;
		type = obj.type;
	}
	public static JSONObject Create() {
#if POOLING
		JSONObject result = null;
		while(result == null && releaseQueue.Count > 0) {
			result = releaseQueue.Dequeue();
#if DEV
			//The following cases should NEVER HAPPEN (but they do...)
			if(result == null)
				Debug.WriteLine("wtf " + releaseQueue.Count);
			else if(result.list != null)
				Debug.WriteLine("wtflist " + result.list.Count);
#endif
		}
		if(result != null)
			return result;
#endif
		return new JSONObject();
	}
	public static JSONObject Create(Type t) {
		JSONObject obj = Create();
		obj.type = t;
		switch(t) {
			case Type.ARRAY:
				obj.list = new List<JSONObject>();
				break;
			case Type.OBJECT:
				obj.list = new List<JSONObject>();
				obj.keys = new List<string>();
				break;
		}
		return obj;
	}
	public static JSONObject Create(bool val) {
		JSONObject obj = Create();
		obj.type = Type.BOOL;
		obj.b = val;
		return obj;
	}
	public static JSONObject Create(float val) {
		JSONObject obj = Create();
		obj.type = Type.NUMBER;
		obj.n = val;
		return obj;
	}
	public static JSONObject Create(int val) {
		JSONObject obj = Create();
		obj.