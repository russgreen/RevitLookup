// Copyright (c) Lookup Foundation and Contributors
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// THIS PROGRAM IS PROVIDED "AS IS" AND WITH ALL FAULTS.
// NO IMPLIED WARRANTY OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE IS PROVIDED.
// THERE IS NO GUARANTEE THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.

using System.Reflection;
using System.Windows;
using System.Windows.Media;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public class DependencyObjectDescriptor(DependencyObject dependencyObject) : Descriptor, IDescriptorResolver, IDescriptorExtension
{
    public virtual Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(DependencyObject.GetLocalValueEnumerator) => ResolveGetLocalValueEnumerator,
            _ => null
        };

        IVariant ResolveGetLocalValueEnumerator()
        {
            return Variants.Empty<LocalValueEnumerator?>();
        }
    }

    public virtual void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("GetVisualParent", RegisterGetVisualParent);
        manager.Register("GetVisualChild", RegisterGetVisualChild);
        manager.Register("GetVisualChildrenCount", RegisterGetVisualChildrenCount);
        manager.Register("GetLogicalParent", RegisterGetLogicalParent);
        manager.Register("GetLogicalChildren", RegisterGetLogicalChildren);
        return;

        IVariant RegisterGetVisualParent()
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            return Variants.Value(parent);
        }

        IVariant RegisterGetVisualChildrenCount()
        {
            var count = VisualTreeHelper.GetChildrenCount(dependencyObject);
            return Variants.Value(count);
        }

        IVariant RegisterGetVisualChild()
        {
            var count = VisualTreeHelper.GetChildrenCount(dependencyObject);
            var variants = Variants.Values<DependencyObject>(count);
            for (var i = 0; i < count; i++)
            {
                variants.Add(VisualTreeHelper.GetChild(dependencyObject, i));
            }

            return variants.Consume();
        }
        
        IVariant RegisterGetLogicalParent()
        {
            var parent = LogicalTreeHelper.GetParent(dependencyObject);
            return Variants.Value(parent);
        }

        IVariant RegisterGetLogicalChildren()
        {
            var count = LogicalTreeHelper.GetChildren(dependencyObject);
            return Variants.Value(count);
        }
    }
}