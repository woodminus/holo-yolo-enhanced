
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// Encapsulates the shape detection queries of the understanding dll.
    /// Shapes are defined by the user with AddShape and the analysis is 
    /// initiated with ActivateShapeAnalysis. These queries will not be 
    /// valid until after scanning is finalized.
    /// 
    /// Shape definitions are composed of a list of components and a list
    /// of shape constraints which defining requirements between the 
    /// components. Each component is defined by a list of its own shape 
    /// component constraints.
    /// </summary>
    public static class SpatialUnderstandingDllShapes
    {
        /// <summary>
        /// Result structure returned by shape queries
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ShapeResult
        {
            public Vector3 position;
            public Vector3 halfDims;
        };

        /// <summary>
        /// Types of shape component constraints
        /// </summary>
        public enum ShapeComponentConstraintType
        {
            SurfaceNotPartOfShape,

            SurfaceHeight_Min,
            SurfaceHeight_Max,
            SurfaceHeight_Between,
            SurfaceHeight_Is,

            SurfaceCount_Min,
            SurfaceCount_Max,
            SurfaceCount_Between,
            SurfaceCount_Is,

            SurfaceArea_Min,
            SurfaceArea_Max,
            SurfaceArea_Between,
            SurfaceArea_Is,

            IsRectangle,
            RectangleSize_Min,
            RectangleSize_Max,
            RectangleSize_Between,
            RectangleSize_Is,

            RectangleLength_Min,
            RectangleLength_Max,
            RectangleLength_Between,
            RectangleLength_Is,

            RectangleWidth_Min,
            RectangleWidth_Max,
            RectangleWidth_Between,
            RectangleWidth_Is,

            IsSquare,
            SquareSize_Min,
            SquareSize_Max,
            SquareSize_Between,
            SquareSize_Is,

            IsCircle,
            CircleRadius_Min,
            CircleRadius_Max,
            CircleRadius_Between,
            CircleRadius_Is,
        };

        /// <summary>
        /// A shape component constraint. This includes its type enum and 
        /// its type specific parameters.
        /// 
        /// Static construction functions contained in this class can be used
        /// to construct a list of component constraints.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ShapeComponentConstraint
        {
            /// <summary>
            /// Constructs a constraint requiring the component to not be a part of a specified shape
            /// </summary>
            /// <returns>Constructed component constraint</returns>
            public static ShapeComponentConstraint Create_SurfaceNotPartOfShape(string shapeName)
            {
                ShapeComponentConstraint constraint = new ShapeComponentConstraint();
                constraint.Type = ShapeComponentConstraintType.SurfaceNotPartOfShape;
                constraint.Param_Str_0 = SpatialUnderstanding.Instance.UnderstandingDLL.PinString(shapeName);
                return constraint;
            }

            /// <summary>
            /// Constructs a constraint requiring the component to be a minimum height above the floor
            /// </summary>
            /// <param name="minHeight">Minimum height above the floor</param>
            /// <returns>Constructed component constraint</returns>
            public static ShapeComponentConstraint Create_SurfaceHeight_Min(float minHeight)
            {
                ShapeComponentConstraint constraint = new ShapeComponentConstraint();
                constraint.Type = ShapeComponentConstraintType.SurfaceHeight_Min;
                constraint.Param_Float_0 = minHeight;
                return constraint;
            }

            /// <summary>
            /// Constructs a constraint requiring the component to be a maximum height above the floor
            /// </summary>
            /// <param name="maxHeight">Maximum height above the floor</param>
            /// <returns>Constructed component constraint</returns>
            public static ShapeComponentConstraint Create_SurfaceHeight_Max(float maxHeight)
            {
                ShapeComponentConstraint constraint = new ShapeComponentConstraint();
                constraint.Type = ShapeComponentConstraintType.SurfaceHeight_Max;
                constraint.Param_Float_0 = maxHeight;
                return constraint;
            }

            /// <summary>
            /// Constructs a constraint requiring the component to be within a height range above the floor
            /// </summary>
            /// <param name="minHeight">Minimum height above the floor</param>
            /// <param name="maxHeight">Maximum height above the floor</param>
            /// <returns>Constructed component constraint</returns>
            public static ShapeComponentConstraint Create_SurfaceHeight_Between(float minHeight, float maxHeight)
            {
                ShapeComponentConstraint constraint = new ShapeComponentConstraint();
                constraint.Type = ShapeComponentConstraintType.SurfaceHeight_Between;
                constraint.Param_Float_0 = minHeight;
                constraint.Param_Float_1 = maxHeight;
                return constraint;
            }

            /// <summary>
            /// Constructs a constraint requiring the component to be a specific height above the floor
            /// </summary>
            /// <param name="height">Required height above the floor</param>
            /// <returns>Constructed component constraint</returns>
            public static ShapeComponentConstraint Create_SurfaceHeight_Is(float height)
            {
                ShapeComponentConstraint constraint = new ShapeComponentConstraint();
                constraint.Type = ShapeComponentConstraintType.SurfaceHeight_Is;
                constraint.Param_Float_0 = height;
                return constraint;
            }

            /// <summary>
            /// Constructs a constraint requiring the component to be a minimum number of discrete flat surfaces
            /// </summary>
            /// <param name="minCount">Minimum number of discrete surfaces</param>
            /// <returns>Constructed component constraint</returns>
            public static ShapeComponentConstraint Create_SurfaceCount_Min(int minCount)
            {
                ShapeComponentConstraint constraint = new ShapeComponentConstraint();
                constraint.Type = ShapeComponentConstraintType.SurfaceCount_Min;
                constraint.Param_Int_0 = minCount;
                return constraint;
            }

            /// <summary>
            /// Constructs a constraint requiring the component to be a maximum number of discrete flat surfaces
            /// </summary>
            /// <param name="maxCount">Maximum number of discrete surfaces</param>
            /// <returns>Constructed component constraint</returns>
            public static ShapeComponentConstraint Create_SurfaceCount_Max(int maxCount)
            {
                ShapeComponentConstraint constraint = new ShapeComponentConstraint();
                constraint.Type = ShapeComponentConstraintType.SurfaceCount_Max;
                constraint.Param_Int_0 = maxCount;
                return constraint;
            }

            /// <summary>
            /// Constructs a constraint requiring the component to be a composed of a number of 
            /// discrete flat surfaces between a specified range
            /// </summary>
            /// <param name="minCount">Minimum number of discrete surfaces</param>
            /// <param name="maxCount">Maximum number of discrete surfaces</param>
            /// <returns>Constructed component constraint</returns>
            public static ShapeComponentConstraint Create_SurfaceCount_Between(int minCount, int maxCount)
            {
                ShapeComponentConstraint constraint = new ShapeComponentConstraint();
                constraint.Type = ShapeComponentConstraintType.SurfaceCount_Between;
                constraint.Param_Int_0 = minCount;
                constraint.Param_Int_1 = maxCount;
                return constraint;
            }

            /// <summary>
            /// Constructs a constraint requiring the component to be a composed of a number of 
            /// discrete flat surfaces of the count specified
            /// </summary>