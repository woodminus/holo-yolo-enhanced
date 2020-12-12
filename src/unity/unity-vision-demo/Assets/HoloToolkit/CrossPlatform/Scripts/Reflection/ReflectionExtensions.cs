// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#if UNITY_METRO && !UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public static class ReflectionExtensions
{
    public static EventInfo GetEvent(this Type type, string eventName)
    {
        return type.GetRuntimeEvent(eventName);
    }

    public static MethodInfo GetMethod(this Type type, string methodName)
    {
        return GetMethod(type, methodName, (BindingFlags)0x0);
    }

    public static MethodInfo GetMethod(this Type type, string methodName, BindingFlags flags)
    {
        var result = type.GetTypeInfo().GetDeclaredMethod(methodName);
        if (((flags & BindingFlags.FlattenHierarchy) != 0) && result == null)
        {
            var baseType = type.GetBaseType();
            if (baseType != null)
            {
                return GetMethod(baseType, methodName, flags);
            }
        }

        return result;
    }

    public static MethodInfo GetMethod(this Type type, string methodName, BindingFlags bindingAttr,  Object binder, Type[] parameters, Object[] modifiers)
    {
        var result = type.GetTypeInfo().GetDeclaredMethod(methodName);
        if (result == null)
        {
            var baseType = type.GetBaseType();
            if (baseType != null)
            {
                return GetMethod(baseType, methodName, bindingAttr, binder, parameters, modifiers);
            }
        }

        return result;
    }

    public static MethodInfo GetMethod(this Type type, string methodName, Type[] parameters)
    {
        return GetMethods(type).Where(m => m.Name == methodName).FirstOrDefault(
            m =>
            {
                var types = m.GetParameters().Select(p => p.ParameterType).ToArray();
                if (types.Length == parameters.Length)
                {
                    for (int idx = 0; idx < types.Length; idx++)
                    {
                        if (types[idx] != parameters[idx])
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        );
    }

    public static IEnumerable<MethodInfo> GetMethods(this Type type)
    {
        return GetMethods(type, (BindingFlags)0x0);
    }

    public static IEnumerable<MethodInfo> GetMethods(this Type type, BindingFlags flags)
    {
        return type.GetTypeInfo().GetMethods(flags);
    }

    public static IEnumerable<MethodInfo> GetMethods(th