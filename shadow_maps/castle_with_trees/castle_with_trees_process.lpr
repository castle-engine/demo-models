{ -*- compile-command: "make" -*- }
{
  Copyright 2010-2017 Michalis Kamburelis.

  This file is part of "Castle Game Engine".

  "Castle Game Engine" is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  "Castle Game Engine" is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with "Castle Game Engine"; if not, write to the Free Software
  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA

  ----------------------------------------------------------------------------
}

{ Process VRML produced by Blender ( castle_with_trees.wrl ) into X3D with
  shadow maps nodes ( castle_with_trees_processed.x3dv ).
  Run like

    castle-engine compile
    ./castle_with_trees_process castle_with_trees.wrl  > castle_with_trees_processed.x3dv
}

uses SysUtils, CastleUtils, CastleClassUtils, X3DNodes, X3DLoad,
  CastleStringUtils, X3DFields, CastleParameters,
  CastleURIUtils, CastleApplicationProperties;

const
  ShadowMapSize = 1024;

{ Find Blender's light with given name, and add X3D fields setting
  projection near/far for shadow maps. }
procedure SetLightProjection(Model: TX3DNode;
  const LightName: string;
  const ProjectionNear, ProjectionFar: Single;
  out L: TAbstractPunctualLightNode);
var
  SM: TGeneratedShadowMapNode;
begin
  L := Model.FindNodeByName(TAbstractPunctualLightNode, LightName, false)
    as TAbstractPunctualLightNode;
  L.FdProjectionNear.Value := ProjectionNear;
  L.FdProjectionFar.Value := ProjectionFar;

  { add defaultShadowMap, to set size to ShadowMapSize }
  SM := TGeneratedShadowMapNode.Create;
  L.FdDefaultShadowMap.Value := SM;
  SM.Update := upAlways;
  SM.FdSize.Value := ShadowMapSize;
end;

{ Find the given Blender mesh, and set corresponding VRML Shapes
  to receive shadows from the given light. }
procedure MakeShadowMapReceiver(Model: TX3DNode;
  const MeshName: string; Light: TAbstractPunctualLightNode);
var
  Group: TGroupNode;
  ShapeNum: Integer;
  Shape: TShapeNode;
begin
  Group := Model.TryFindNodeByName(TGroupNode, 'ME_' + MeshName, false)
    as TGroupNode;
  if Group = nil then
    { Retry with MOD_ prefix, exporter adds this to objects with modifiers. }
    Group := Model.TryFindNodeByName(TGroupNode, 'ME_MOD_' + MeshName, false)
      as TGroupNode;
  Check(Group <> nil, 'Cannot find Blender mesh ' + MeshName);

  { Within a single Blender ME_xxx there may be many VRML Shapes,
    when one Blender mesh uses multiple materials. }
  for ShapeNum := 0 to Group.FdChildren.Count - 1 do
  begin
    Shape := Group.FdChildren.Items[ShapeNum] as TShapeNode;

    { make sure Shape has some (valid) Appearance }
    if Shape.Appearance = nil then
      Shape.Appearance := TAppearanceNode.Create;

    Shape.Appearance.FdReceiveShadows.Add(Light);
  end;
end;

var
  URL: string;
  Model: TX3DRootNode;
  Light: TAbstractPunctualLightNode;
begin
  Parameters.CheckHigh(1);
  URL := Parameters[1];

  ApplicationProperties.OnWarning.Add(@ApplicationProperties.WriteWarningOnConsole);

  Model := LoadNode(URL);
  try
    Model.ForceSaveAsX3D;

    SetLightProjection(Model, 'Lamp_001', 8, 31, Light);
//    SetLightProjection(Model, 'Lamp', 10, 34, Light);
//    SetLightProjection(Model, 'Lamp_002', 8, 90, Light);
    MakeShadowMapReceiver(Model, 'Plane', Light);
    MakeShadowMapReceiver(Model, 'Plane_002', Light);
    MakeShadowMapReceiver(Model, 'wall1', Light);
    MakeShadowMapReceiver(Model, 'Tube_002', Light);
    MakeShadowMapReceiver(Model, 'oak_004', Light);

    { For visualizing shadow map depths, it's looks best to put shadow map
      on pretty much everything. }
    //   MakeShadowMapReceiver(Model, 'wall1_004', Light);
    //   MakeShadowMapReceiver(Model, 'wall1_003', Light);
    //   MakeShadowMapReceiver(Model, 'wall1_002', Light);
    //   MakeShadowMapReceiver(Model, 'wall1_001', Light);
    //   MakeShadowMapReceiver(Model, 'oak', Light); {< oak leaves }

    SaveNode(Model, StdOutStream, 'model/x3d+vrml',
      'castle_with_trees_process (from Castle Game Engine demo models)',
      ExtractURIName(URL));
  finally FreeAndNil(Model) end;
end.
