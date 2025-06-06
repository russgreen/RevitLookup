﻿// Copyright (c) Lookup Foundation and Contributors
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

using WixSharp;
using WixSharp.CommonTasks;
using File = WixSharp.File;

namespace Installer;

public static class Generator
{
    /// <summary>
    ///     Generates Wix entities for the installer.
    /// </summary>
    public static WixEntity[] GenerateWixEntities(IEnumerable<string> args, Version version)
    {
        var entities = new List<WixEntity>();
        foreach (var directory in args)
        {
            Console.WriteLine($"Installer files for version '{version}':");
            GenerateRootEntities(directory, entities);
        }

        return entities.ToArray();
    }

    /// <summary>
    ///     Generates root entities.
    /// </summary>
    private static void GenerateRootEntities(string directory, ICollection<WixEntity> entities)
    {
        foreach (var file in Directory.GetFiles(directory))
        {
            if (!FilterEntities(file)) continue;

            Console.WriteLine($"'{file}'");
            entities.Add(new File(file));
        }

        foreach (var folder in Directory.GetDirectories(directory))
        {
            var folderName = Path.GetFileName(folder);
            var entity = new Dir(folderName);
            entities.Add(entity);

            GenerateSubEntities(folder, entity);
        }
    }

    /// <summary>
    ///     Generates nested entities recursively.
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="parent"></param>
    private static void GenerateSubEntities(string directory, Dir parent)
    {
        foreach (var file in Directory.GetFiles(directory))
        {
            if (!FilterEntities(file)) continue;

            Console.WriteLine($"'{file}'");
            parent.AddFile(new File(file));
        }

        foreach (var subfolder in Directory.GetDirectories(directory))
        {
            var folderName = Path.GetFileName(subfolder);
            var entity = new Dir(folderName);
            parent.AddDir(entity);

            GenerateSubEntities(subfolder, entity);
        }
    }

    /// <summary>
    ///     Filter installer files and exclude from output. 
    /// </summary>
    private static bool FilterEntities(string file)
    {
        return !file.EndsWith(".pdb");
    }
}