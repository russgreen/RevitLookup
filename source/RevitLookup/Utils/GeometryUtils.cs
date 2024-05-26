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

namespace RevitLookup.Utils;

public static class GeometryUtils
{
    public static XYZ GetMeshVertexNormal(Mesh mesh, int index, DistributionOfNormals normalDistribution)
    {
        switch (normalDistribution)
        {
            case DistributionOfNormals.AtEachPoint:
                return mesh.GetNormal(index);
            case DistributionOfNormals.OnEachFacet:
                var vertex = mesh.Vertices[index];
                for (var i = 0; i < mesh.NumTriangles; i++)
                {
                    var triangle = mesh.get_Triangle(i);
                    var triangleVertex = triangle.get_Vertex(0);
                    if (triangleVertex.IsAlmostEqualTo(vertex)) return mesh.GetNormal(i);
                    triangleVertex = triangle.get_Vertex(1);
                    if (triangleVertex.IsAlmostEqualTo(vertex)) return mesh.GetNormal(i);
                    triangleVertex = triangle.get_Vertex(2);
                    if (triangleVertex.IsAlmostEqualTo(vertex)) return mesh.GetNormal(i);
                }
                
                return XYZ.Zero;
            case DistributionOfNormals.OnePerFace:
                return mesh.GetNormal(0);
            default:
                throw new ArgumentOutOfRangeException(nameof(normalDistribution), normalDistribution, null);
        }
    }
    
    public static List<XYZ> TessellateCircle(XYZ center, XYZ normal, double raduis)
    {
        var vertices = new List<XYZ>();
        var segmentCount = InterpolateSegmentsCount(raduis);
        var xDirection = normal.CrossProduct(XYZ.BasisZ).Normalize() * raduis;
        if (xDirection.IsZeroLength())
        {
            xDirection = normal.CrossProduct(XYZ.BasisX).Normalize() * raduis;
        }
        
        var yDirection = normal.CrossProduct(xDirection).Normalize() * raduis;
        
        for (var i = 0; i < segmentCount; i++)
        {
            var angle = 2 * Math.PI * i / segmentCount;
            var vertex = center + xDirection * Math.Cos(angle) + yDirection * Math.Sin(angle);
            vertices.Add(vertex);
        }
        
        return vertices;
    }
    
    private static int InterpolateSegmentsCount(double diameter)
    {
        const int minSegments = 6;
        const int maxSegments = 33;
        const double minDiameter = 0.1 / 12d;
        const double maxDiameter = 3 / 12d;
        
        if (diameter <= minDiameter) return minSegments;
        if (diameter >= maxDiameter) return maxSegments;
        
        var normalDiameter = (diameter - minDiameter) / (maxDiameter - minDiameter);
        return (int) (minSegments + normalDiameter * (maxSegments - minSegments));
    }
}