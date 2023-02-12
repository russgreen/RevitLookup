﻿// Copyright 2003-2022 by Autodesk, Inc.
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

using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Toolkit.External;
using RevitLookup.Services.Contracts;
using RevitLookup.Services.Enums;

namespace RevitLookup.Commands;

// [UsedImplicitly]
// [Transaction(TransactionMode.Manual)]
// public class DashboardCommand : ExternalCommand
// {
//     public override void Execute()
//     {
//         var externalServices = ExternalServiceRegistry.GetServices();
//         var window = Host.GetService<IWindow>();
//         window.Show(UiApplication.MainWindowHandle);
//         window.Scope.GetService<INavigationService>().Navigate(typeof(DashboardView));
//     }
// }

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class SnoopSelectionCommand : ExternalCommand
{
    public override void Execute()
    {
        Execute(UiApplication);
    }

    /// <summary>
    ///     Modify tab command support
    /// </summary>
    public static async void Execute(UIApplication application)
    {
        var window = Host.GetService<IWindow>();
        await window.Scope.GetService<ISnoopService>()!.Snoop(SnoopableType.Selection);
        window.Show(application.MainWindowHandle);
    }
}

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class SnoopLinkedElementCommand : ExternalCommand
{
    public override async void Execute()
    {
        var window = Host.GetService<IWindow>();
        await window.Scope.GetService<ISnoopService>()!.Snoop(SnoopableType.LinkedElement);
        window.Show(UiApplication.MainWindowHandle);
    }
}

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class SnoopFaceCommand : ExternalCommand
{
    public override async void Execute()
    {
        var window = Host.GetService<IWindow>();
        await window.Scope.GetService<ISnoopService>()!.Snoop(SnoopableType.Face);
        window.Show(UiApplication.MainWindowHandle);
    }
}

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class SnoopEdgeCommand : ExternalCommand
{
    public override async void Execute()
    {
        var window = Host.GetService<IWindow>();
        await window.Scope.GetService<ISnoopService>()!.Snoop(SnoopableType.Edge);
        window.Show(UiApplication.MainWindowHandle);
    }
}

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class SnoopPointCommand : ExternalCommand
{
    public override async void Execute()
    {
        var window = Host.GetService<IWindow>();
        await window.Scope.GetService<ISnoopService>()!.Snoop(SnoopableType.Point);
        window.Show(UiApplication.MainWindowHandle);
    }
}

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class SnoopSubElementCommand : ExternalCommand
{
    public override async void Execute()
    {
        var window = Host.GetService<IWindow>();
        await window.Scope.GetService<ISnoopService>()!.Snoop(SnoopableType.SubElement);
        window.Show(UiApplication.MainWindowHandle);
    }
}