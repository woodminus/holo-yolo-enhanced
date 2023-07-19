
﻿// Copyright Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using System.Collections;

public class PlaneTargetGroup : MonoBehaviour
{
    [Tooltip("Enter in same target consecutively to turn on velocity tracking for that target.")]
    public Transform[] Targets;
    