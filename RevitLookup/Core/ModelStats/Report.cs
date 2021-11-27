#region Header

//
// Copyright 2003-2021 by Autodesk, Inc. 
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
//

#endregion // Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using Autodesk.Revit.DB;

namespace RevitLookup.Core.ModelStats
{
    /// <summary>
    ///     Summary description for Report.
    /// </summary>
    public class Report
    {
        private readonly ArrayList _categoryCounts = new();

        // data members
        private readonly ArrayList _rawObjCounts = new();
        private readonly ArrayList _symRefCounts = new();

        /// <summary>
        ///     Find a RawObjCount node for the the given type of object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private RawObjCount FindRawObjNode(Type objType)
        {
            return _rawObjCounts
                .Cast<RawObjCount>()
                .FirstOrDefault(tmpNode => tmpNode.MClassType == objType);
        }

        private void RawObjStats(object obj)
        {
            var tmpNode = FindRawObjNode(obj.GetType());
            if (tmpNode == null)
            {
                tmpNode = new RawObjCount
                {
                    MClassType = obj.GetType()
                };
                _rawObjCounts.Add(tmpNode);
            }

            tmpNode.Objs.Add(obj);
        }

        private CategoryCount FindCategoryNode(Category cat)
        {
            Debug.Assert(cat != null); // don't call unless you've already checked this

            return _categoryCounts
                .Cast<CategoryCount>()
                .FirstOrDefault(tmpNode => tmpNode.MCategory == cat);
        }

        private void CategoryStats(Element elem)
        {
            if (elem.Category == null) // some elements don't belong to a category
                return;

            var tmpNode = FindCategoryNode(elem.Category);
            if (tmpNode == null)
            {
                tmpNode = new CategoryCount
                {
                    MCategory = elem.Category
                };
                _categoryCounts.Add(tmpNode);
            }

            tmpNode.Objs.Add(elem);
        }

        private SymbolCount FindSymbolNode(ElementType sym)
        {
            return _symRefCounts
                .Cast<SymbolCount>()
                .FirstOrDefault(tmpNode => tmpNode.MSymbol.Id.IntegerValue == sym.Id.IntegerValue);
        }

        private ElementType GetSymbolRef(Element elem)
        {
            switch (elem)
            {
                case FamilyInstance famInst:
                    return famInst.Symbol;
                case Floor floor:
                    return floor.FloorType;
                case Group group:
                    return group.GroupType;
                case Wall wall:
                    return wall.WallType;
                default:
                    return null; // nothing we know about
            }
        }

        private void SymbolRefStats(Element elem)
        {
            // if it is a Symbol element, just make an entry in our map
            // and get out.
            if (elem is ElementType sym)
            {
                var tmpNode = FindSymbolNode(sym);
                if (tmpNode == null)
                {
                    tmpNode = new SymbolCount
                    {
                        MSymbol = sym
                    };
                    _symRefCounts.Add(tmpNode);
                }

                return;
            }

            // it isn't a Symbol, so we need to check if it is something that
            // references a Symbol.
            sym = GetSymbolRef(elem);
            if (sym != null)
            {
                var tmpNode = FindSymbolNode(sym);
                if (tmpNode == null)
                {
                    tmpNode = new SymbolCount
                    {
                        MSymbol = sym
                    };
                    _symRefCounts.Add(tmpNode);
                }

                tmpNode.Refs.Add(elem);
            }
        }


        private void ProcessElements(Document doc)
        {
            var fec = new FilteredElementCollector(doc);
            var elementsAreWanted = new ElementClassFilter(typeof(Element));
            fec.WherePasses(elementsAreWanted);
            if (fec.ToElements() is not List<Element> elements) return;
            foreach (var element in elements)
            {
                RawObjStats(element);

                if (element != null)
                {
                    SymbolRefStats(element);
                    CategoryStats(element);
                }
            }
        }

        /// <summary>
        ///     Create the XML Report for this document
        /// </summary>
        /// <param name="reportPath"></param>
        /// <param name="elemIter"></param>
        public void XmlReport(string reportPath, Document doc)
        {
            ProcessElements(doc); // index all of the elements

            var stream = new XmlTextWriter(reportPath, Encoding.UTF8);
            stream.Formatting = Formatting.Indented;
            stream.IndentChar = '\t';
            stream.Indentation = 1;

            stream.WriteStartDocument();

            stream.WriteStartElement("Project");
            stream.WriteAttributeString("title", doc.Title);
            stream.WriteAttributeString("path", doc.PathName);

            XmlReportRawCounts(stream);
            XmlReportCategoryCounts(stream);
            XmlReportSymbolRefCounts(stream);

            stream.WriteEndElement(); // "Project"
            stream.WriteEndDocument();

            stream.Close();
        }

        private void XmlReportRawCounts(XmlTextWriter stream)
        {
            stream.WriteStartElement("RawCounts");

            foreach (RawObjCount tmpNode in _rawObjCounts)
            {
                // write summary stats for this class type
                stream.WriteStartElement("ClassType");
                stream.WriteAttributeString("name", tmpNode.MClassType.Name);
                stream.WriteAttributeString("fullName", tmpNode.MClassType.FullName);
                stream.WriteAttributeString("count", tmpNode.Objs.Count.ToString());
                // list a reference to each element of this type
                foreach (var tmpObj in tmpNode.Objs)
                    if (tmpObj is Element tmpElem)
                    {
                        stream.WriteStartElement("ElementRef");
                        stream.WriteAttributeString("idRef", tmpElem.Id.IntegerValue.ToString());
                        stream.WriteEndElement(); // ElementRef
                    }

                stream.WriteEndElement(); // ClassType
            }

            stream.WriteEndElement(); // RawCounts
        }

        private void XmlReportCategoryCounts(XmlTextWriter stream)
        {
            stream.WriteStartElement("Categories");

            foreach (CategoryCount tmpNode in _categoryCounts)
            {
                // write summary stats for this category
                stream.WriteStartElement("Category");
                stream.WriteAttributeString("name", tmpNode.MCategory.Name);
                stream.WriteAttributeString("count", tmpNode.Objs.Count.ToString());
                // list a reference to each element of this type
                foreach (Element tmpElem in tmpNode.Objs)
                {
                    stream.WriteStartElement("ElementRef");
                    stream.WriteAttributeString("idRef", tmpElem.Id.IntegerValue.ToString());
                    stream.WriteEndElement(); // ElementRef
                }

                stream.WriteEndElement(); // Category
            }

            stream.WriteEndElement(); // Categories
        }

        private void XmlReportSymbolRefCounts(XmlTextWriter stream)
        {
            stream.WriteStartElement("Symbols");

            foreach (SymbolCount tmpNode in _symRefCounts)
            {
                // write summary stats for this Symbol
                stream.WriteStartElement("Symbol");
                stream.WriteAttributeString("name", tmpNode.MSymbol.Name);
                stream.WriteAttributeString("symbolType", tmpNode.MSymbol.GetType().Name);
                stream.WriteAttributeString("refCount", tmpNode.Refs.Count.ToString());
                // list a reference to each element of this type
                foreach (Element tmpElem in tmpNode.Refs)
                {
                    stream.WriteStartElement("ElementRef");
                    stream.WriteAttributeString("idRef", tmpElem.Id.IntegerValue.ToString());
                    stream.WriteEndElement(); // ElementRef
                }

                stream.WriteEndElement(); // Symbol
            }

            stream.WriteEndElement(); // Symbols
        }

        private void XmlReportWriteElement(XmlTextWriter stream, Element elem)
        {
            if (elem.Category == null) return;

            stream.WriteStartElement("BldgElement");
            stream.WriteAttributeString("category", elem.Category.Name);
            stream.WriteAttributeString("id", elem.Id.IntegerValue.ToString());
            stream.WriteAttributeString("name", elem.Name);

            var paramSet = elem.Parameters;
            foreach (Parameter tmpObj in paramSet)
            {
                stream.WriteStartElement("Param");
                stream.WriteAttributeString("name", tmpObj.Definition.Name);
                switch (tmpObj.StorageType)
                {
                    case StorageType.Double:
                        stream.WriteAttributeString("dataType", "double");
                        stream.WriteAttributeString("value", tmpObj.AsDouble().ToString());
                        break;
                    case StorageType.ElementId:
                        stream.WriteAttributeString("dataType", "elemId");
                        stream.WriteAttributeString("value", tmpObj.AsElementId().IntegerValue.ToString());
                        break;
                    case StorageType.Integer:
                        stream.WriteAttributeString("dataType", "int");
                        stream.WriteAttributeString("value", tmpObj.AsInteger().ToString());
                        break;
                    case StorageType.String:
                        stream.WriteAttributeString("dataType", "string");
                        stream.WriteAttributeString("value", tmpObj.AsString());
                        break;
                    case StorageType.None:
                        stream.WriteAttributeString("dataType", "none");
                        stream.WriteAttributeString("value", "???");
                        break;
                }

                stream.WriteEndElement(); // "Param"
            }

            stream.WriteEndElement(); // "BldgElement"
        }
    }
}