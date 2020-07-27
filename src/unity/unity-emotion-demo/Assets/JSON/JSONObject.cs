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
	pub