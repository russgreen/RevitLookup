RevitLookup provides functionality to display methods in the user interface that do not exist in the Revit API or implemented as a Util classes. 
These extensions expose useful functionality in the context of specific objects.

> [!NOTE]  
> The available extensions may vary depending on the version of Revit you are using.

The table below lists the extensions that are available in RevitLookup:

| Type           | Extension                                 | API method                                                                       |
|:---------------|-------------------------------------------|----------------------------------------------------------------------------------|
| Application    | GetFormulaFunctions                       | FormulaManager.GetFormulaFunctions                                               |
| Application    | GetFormulaOperators                       | FormulaManager.GetFormulaOperators                                               |
| Application    | GetMacroManager                           | MacroManager.GetMacroManager                                                     |
| UIApplication  | CurrentTheme                              | UIThemeManager.CurrentTheme                                                      |
| UIApplication  | CurrentCanvasTheme                        | UIThemeManager.CurrentCanvasTheme                                                |
| UIApplication  | FollowSystemColorTheme                    | UIThemeManager.FollowSystemColorTheme                                            |
| Document       | GetLightGroupManager                      | LightGroupManager.GetLightGroupManager                                           |
| Document       | GetTemporaryGraphicsManager               | TemporaryGraphicsManager.GetTemporaryGraphicsManager                             |
| Document       | GetAnalyticalToPhysicalAssociationManager | AnalyticalToPhysicalAssociationManager.GetAnalyticalToPhysicalAssociationManager |
| Document       | GetFamilySizeTableManager                 | FamilySizeTableManager.GetFamilySizeTableManager                                 |
| Document       | GetLightFamily                            | LightFamily.GetLightFamily                                                       |
| Document       | GetMacroManager                           | MacroManager.GetMacroManager                                                     |
| Element        | CanBeMirrored                             | ElementTransformUtils.CanBeMirrored                                              |
| Element        | GetJoinedElements                         | JoinGeometryUtils.GetJoinedElements                                              |
| Element        | GetCuttingSolids                          | SolidSolidCutUtils.GetCuttingSolids                                              |
| Element        | GetSolidsBeingCut                         | SolidSolidCutUtils.GetSolidsBeingCut                                             |
| Element        | IsAllowedForSolidCut                      | SolidSolidCutUtils.IsAllowedForSolidCut                                          |
| Element        | IsElementFromAppropriateContext           | SolidSolidCutUtils.IsElementFromAppropriateContext                               |
| Element        | GetCheckoutStatus                         | WorksharingUtils.GetCheckoutStatus                                               |
| Element        | GetWorksharingTooltipInfo                 | WorksharingUtils.GetWorksharingTooltipInfo                                       |
| Element        | GetModelUpdatesStatus                     | WorksharingUtils.GetModelUpdatesStatus                                           |
| Element        | AreElementsValidForCreateParts            | PartUtils.AreElementsValidForCreateParts                                         |
| Element        | CanDeleteElement                          | DocumentValidation.CanDeleteElement                                              |
| FamilyInstance | GetInstancePlacementPointElementRefIds    | AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds            |
| FamilyInstance | IsAdaptiveComponentInstance               | AdaptiveComponentInstanceUtils.IsAdaptiveComponentInstance                       |
| Family         | GetFamilySizeTableManager                 | FamilySizeTableManager.GetFamilySizeTableManager                                 |
| Family         | FamilyCanConvertToFaceHostBased           | FamilyUtils.FamilyCanConvertToFaceHostBased                                      |
| Family         | GetProfileSymbols                         | FamilyUtils.GetProfileSymbols                                                    |
| LightFamily    | GetLightTypeName                          | LightFamily.GetLightTypeName                                                     |
| LightFamily    | GetLightType                              | LightFamily.GetLightType                                                         |
| HostObject     | GetBottomFaces                            | HostObjectUtils.GetBottomFaces                                                   |
| HostObject     | GetTopFaces                               | HostObjectUtils.GetTopFaces                                                      |
| HostObject     | GetSideFaces                              | HostObjectUtils.GetSideFaces                                                     |
| View           | GetSpatialFieldManager                    | SpatialFieldManager.GetSpatialFieldManager                                       |
| View           | GetAllPlacedInstances                     | -                                                                                |
| Pipe           | HasOpenConnector                          | PlumbingUtils.HasOpenConnector                                                   |
| Parameter      | AsBool                                    | -                                                                                |
| Parameter      | AsColor                                   | -                                                                                |
| Parameter      | GetAssociatedFamilyParameter              | FamilyManager.GetAssociatedFamilyParameter                                       |
| ForgeTypeId    | ToLabel                                   | Returns user visible label                                                       |
| ForgeTypeId    | IsSymbol                                  | UnitUtils.IsSymbol                                                               |
| ForgeTypeId    | IsUnit                                    | UnitUtils.IsUnit                                                                 |
| ForgeTypeId    | IsSpec                                    | UnitUtils.IsSpec                                                                 |
| Category       | GetElements                               | -                                                                                |
| Schema         | GetElements                               | -                                                                                |
| Color          | Name                                      | -                                                                                |
| Color          | HEX                                       | -                                                                                |
| Color          | HSL                                       | -                                                                                |
| Color          | CMYK                                      | -                                                                                |
| Solid          | SplitVolumes                              | SolidUtils.SplitVolumes                                                          |
| Solid          | IsValidForTessellation                    | SolidUtils.IsValidForTessellation                                                |
| BoundingBoxXYZ | Centroid                                  | -                                                                                |
| BoundingBoxXYZ | Vertices                                  | -                                                                                |
| BoundingBoxXYZ | Volume                                    | -                                                                                |
| BoundingBoxXYZ | SurfaceArea                               | -                                                                                |
| Part           | IsMergedPart                              | PartUtils.IsMergedPart                                                           |
| Part           | IsPartDerivedFromLink                     | PartUtils.IsPartDerivedFromLink                                                  |
| Part           | GetChainLengthToOriginal                  | PartUtils.GetChainLengthToOriginal                                               |
| Part           | GetMergedParts                            | PartUtils.GetMergedParts                                                         |
| Part           | ArePartsValidForDivide                    | PartUtils.ArePartsValidForDivide                                                 |
| Part           | FindMergeableClusters                     | PartUtils.FindMergeableClusters                                                  |
| Part           | ArePartsValidForMerge                     | PartUtils.ArePartsValidForMerge                                                  |
| Part           | GetAssociatedPartMaker                    | PartUtils.GetAssociatedPartMaker                                                 |
| Part           | GetSplittingCurves                        | PartUtils.GetSplittingCurves                                                     |
| Part           | GetSplittingElements                      | PartUtils.GetSplittingElements                                                   |
| Part           | HasAssociatedParts                        | PartUtils.HasAssociatedParts                                                     |
| PartMaker      | GetPartMakerMethodToDivideVolumeFW        | PartUtils.GetPartMakerMethodToDivideVolumeFW                                     |