﻿using Installer;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

const string outputName = "RevitLookup";
const string projectName = "RevitLookup";

var guidMap = new Dictionary<int, string>
{
    { 2015, "1C877362-19E8-4E10-A4B0-802BA88C1F3E" },
    { 2016, "230933BA-3865-41C8-80E1-CFA0EB08B44B" },
    { 2017, "A2402F72-90B0-4803-B783-06487D6BFBEB" },
    { 2018, "2997B545-391C-41B2-A90A-E5C6BB39087A" },
    { 2019, "DBF3C66B-B624-4E0B-B86D-44857B19CD0C" },
    { 2020, "36D21BA1-C945-4D40-83B9-4C2518FC40EA" },
    { 2021, "9B7FD05D-C782-4538-A5D3-04B64AE81FE4" },
    { 2022, "207733B1-1BEA-4603-99EA-EA1E87077F60" },
    { 2023, "2179ECCB-0ED3-4FFF-907D-01C9D57AD20D" },
    { 2024, "2E347D52-D08D-4624-8909-3679D75B9C1D" },
    { 2025, "5182B068-1BE3-42A9-B91D-BF4BEEE81680" },
    { 2026, "468ECD03-68D6-4D99-B04C-13D72AB47CBC" }
};

var versions = Tools.ComputeVersions(args);
if (!guidMap.TryGetValue(versions.RevitVersion, out var guid))
{
    throw new Exception($"Version GUID mapping missing for the specified version: '{versions.RevitVersion}'");
}

var project = new Project
{
    OutDir = "output",
    Name = projectName,
    GUID = new Guid(guid),
    Platform = Platform.x64,
    UI = WUI.WixUI_InstallDir,
    Version = versions.InstallerVersion,
    MajorUpgrade = MajorUpgrade.Default,
    BackgroundImage = @"install\Resources\Icons\BackgroundImage.png",
    BannerImage = @"install\Resources\Icons\BannerImage.png",
    ControlPanelInfo =
    {
        Manufacturer = "Autodesk",
        HelpLink = "https://github.com/lookup-foundation/RevitLookup/issues",
        ProductIcon = @"install\Resources\Icons\ShellIcon.ico"
    }
};

var wixEntities = Generator.GenerateWixEntities(args, versions.AssemblyVersion);
project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);

BuildSingleUserMsi();
BuildMultiUserUserMsi();

void BuildSingleUserMsi()
{
    project.InstallScope = InstallScope.perUser;
    project.OutFileName = $"{outputName}-{versions.AssemblyVersion}-SingleUser";
    project.Dirs =
    [
        new InstallDir($@"%AppDataFolder%\Autodesk\Revit\Addins\{versions.RevitVersion}", wixEntities)
    ];
    project.BuildMsi();
}

void BuildMultiUserUserMsi()
{
    project.InstallScope = InstallScope.perMachine;
    project.OutFileName = $"{outputName}-{versions.AssemblyVersion}-MultiUser";
    project.Dirs =
    [
        new InstallDir($@"%CommonAppDataFolder%\Autodesk\Revit\Addins\{versions.RevitVersion}", wixEntities)
    ];
    project.BuildMsi();
}