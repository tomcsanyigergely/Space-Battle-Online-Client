// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ServerClientMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct ProjectileShotMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static ProjectileShotMessage GetRootAsProjectileShotMessage(ByteBuffer _bb) { return GetRootAsProjectileShotMessage(_bb, new ProjectileShotMessage()); }
  public static ProjectileShotMessage GetRootAsProjectileShotMessage(ByteBuffer _bb, ProjectileShotMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public ProjectileShotMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public UdpMessages.ServerClientMessages.ProjectileShotMessageFix? Fix { get { int o = __p.__offset(4); return o != 0 ? (UdpMessages.ServerClientMessages.ProjectileShotMessageFix?)(new UdpMessages.ServerClientMessages.ProjectileShotMessageFix()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public UdpMessages.Utilities.UInt8Struct? Ammo { get { int o = __p.__offset(6); return o != 0 ? (UdpMessages.Utilities.UInt8Struct?)(new UdpMessages.Utilities.UInt8Struct()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public UdpMessages.Utilities.FloatStruct? ReloadTimeRemaining { get { int o = __p.__offset(8); return o != 0 ? (UdpMessages.Utilities.FloatStruct?)(new UdpMessages.Utilities.FloatStruct()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartProjectileShotMessage(FlatBufferBuilder builder) { builder.StartTable(3); }
  public static void AddFix(FlatBufferBuilder builder, Offset<UdpMessages.ServerClientMessages.ProjectileShotMessageFix> fixOffset) { builder.AddStruct(0, fixOffset.Value, 0); }
  public static void AddAmmo(FlatBufferBuilder builder, Offset<UdpMessages.Utilities.UInt8Struct> ammoOffset) { builder.AddStruct(1, ammoOffset.Value, 0); }
  public static void AddReloadTimeRemaining(FlatBufferBuilder builder, Offset<UdpMessages.Utilities.FloatStruct> reloadTimeRemainingOffset) { builder.AddStruct(2, reloadTimeRemainingOffset.Value, 0); }
  public static Offset<UdpMessages.ServerClientMessages.ProjectileShotMessage> EndProjectileShotMessage(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    builder.Required(o, 4);  // fix
    return new Offset<UdpMessages.ServerClientMessages.ProjectileShotMessage>(o);
  }
};


}