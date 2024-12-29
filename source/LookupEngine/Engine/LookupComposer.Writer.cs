﻿// Copyright 2003-2024 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.Collections;
using System.Reflection;
using LookupEngine.Abstractions;
using LookupEngine.Abstractions.Decomposition;
using LookupEngine.Abstractions.Enums;
using LookupEngine.Formaters;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public partial class LookupComposer
{
    private protected static DecomposedObject CreateNullableDecomposition()
    {
        return new DecomposedObject
        {
            Name = $"{nameof(System)}.{nameof(Object)}",
            RawValue = null,
            TypeName = nameof(Object),
            TypeFullName = $"{nameof(System)}.{nameof(Object)}"
        };
    }

    private static DecomposedObject CreateInstanceDecomposition(object instance, Type type, Descriptor descriptor)
    {
        var formatTypeName = ReflexionFormater.FormatTypeName(type);
        var hasUnknownName = descriptor.Name is null ||
                             type.Namespace is null ||
                             descriptor.Name!.StartsWith(type.Namespace, StringComparison.OrdinalIgnoreCase);

        return new DecomposedObject
        {
            Name = hasUnknownName ? formatTypeName : descriptor.Name!,
            Description = descriptor.Description,
            RawValue = instance,
            TypeName = formatTypeName,
            TypeFullName = $"{type.Namespace}.{formatTypeName}",
            Descriptor = descriptor
        };
    }

    private static DecomposedObject CreateStaticDecomposition(Type type, Descriptor descriptor)
    {
        var formatTypeName = ReflexionFormater.FormatTypeName(type);
        var hasUnknownName = descriptor.Name is null ||
                             type.Namespace is null ||
                             descriptor.Name!.StartsWith(type.Namespace, StringComparison.OrdinalIgnoreCase);

        return new DecomposedObject
        {
            Name = hasUnknownName ? formatTypeName : descriptor.Name!,
            Description = descriptor.Description,
            RawValue = type,
            TypeName = formatTypeName,
            TypeFullName = $"{type.Namespace}.{formatTypeName}",
            Descriptor = descriptor
        };
    }

    private void WriteEnumerableMember(object? value, int index)
    {
        var member = new DecomposedMember
        {
            Depth = _depth,
            Value = CreateValue(nameof(IEnumerable), value),
            Name = $"{ReflexionFormater.FormatTypeName(MemberDeclaringType).Replace("[]", string.Empty)}[{index}]",
            MemberAttributes = MemberAttributes.Property,
            DeclaringTypeName = nameof(IEnumerable),
            DeclaringTypeFullName = $"{nameof(System)}.{nameof(System.Collections)}.{nameof(IEnumerable)}",
        };

        DecomposedMembers.Add(member);
    }

    private protected void WriteExtensionMember(object? value, string name)
    {
        var formatTypeName = ReflexionFormater.FormatTypeName(MemberDeclaringType);

        var member = new DecomposedMember
        {
            Depth = _depth,
            Name = name,
            Value = CreateValue(name, value),
            DeclaringTypeName = formatTypeName,
            DeclaringTypeFullName = $"{MemberDeclaringType.Namespace}.{formatTypeName}",
            MemberAttributes = MemberAttributes.Extension,
            ComputationTime = TimeDiagnoser.GetElapsed().TotalMilliseconds,
            AllocatedBytes = MemoryDiagnoser.GetAllocatedBytes()
        };

        DecomposedMembers.Add(member);
    }

    private void WriteDecompositionMember(object? value, MemberInfo memberInfo)
    {
        var formatTypeName = ReflexionFormater.FormatTypeName(MemberDeclaringType);

        var member = new DecomposedMember
        {
            Depth = _depth,
            Value = CreateValue(memberInfo.Name, value),
            Name = memberInfo.Name,
            DeclaringTypeName = formatTypeName,
            DeclaringTypeFullName = $"{MemberDeclaringType.Namespace}.{formatTypeName}",
            MemberAttributes = ModifiersFormater.FormatAttributes(memberInfo),
            ComputationTime = TimeDiagnoser.GetElapsed().TotalMilliseconds,
            AllocatedBytes = MemoryDiagnoser.GetAllocatedBytes()
        };

        DecomposedMembers.Add(member);
    }

    private void WriteDecompositionMember(object? value, MemberInfo memberInfo, ParameterInfo[] parameters)
    {
        var formatTypeName = ReflexionFormater.FormatTypeName(MemberDeclaringType);

        var member = new DecomposedMember
        {
            Depth = _depth,
            Value = CreateValue(memberInfo.Name, value),
            Name = ReflexionFormater.FormatMemberName(memberInfo, parameters),
            DeclaringTypeName = formatTypeName,
            DeclaringTypeFullName = $"{MemberDeclaringType.Namespace}.{formatTypeName}",
            MemberAttributes = ModifiersFormater.FormatAttributes(memberInfo),
            ComputationTime = TimeDiagnoser.GetElapsed().TotalMilliseconds,
            AllocatedBytes = MemoryDiagnoser.GetAllocatedBytes()
        };

        DecomposedMembers.Add(member);
    }

    private DecomposedValue CreateNullableValue()
    {
        return new DecomposedValue
        {
            RawValue = null,
            Name = string.Empty,
            TypeName = nameof(Object),
            TypeFullName = $"{nameof(System)}.{nameof(Object)}"
        };
    }

    private DecomposedValue CreateValue(string targetMember, object? value)
    {
        if (value is null) return CreateNullableValue();

        value = RedirectValue(value, targetMember, out var valueDescriptor);

        var valueType = value.GetType();
        var formatTypeName = ReflexionFormater.FormatTypeName(valueType);
        var hasUnknownName = valueDescriptor.Name is null ||
                             valueType.Namespace is null ||
                             valueDescriptor.Name.StartsWith(valueType.Namespace, StringComparison.OrdinalIgnoreCase);

        return new DecomposedValue
        {
            RawValue = value,
            Name = hasUnknownName ? formatTypeName : valueDescriptor.Name!,
            Description = valueDescriptor.Description,
            TypeName = formatTypeName,
            TypeFullName = $"{valueType.Namespace}.{formatTypeName}",
            Descriptor = valueDescriptor
        };
    }
}