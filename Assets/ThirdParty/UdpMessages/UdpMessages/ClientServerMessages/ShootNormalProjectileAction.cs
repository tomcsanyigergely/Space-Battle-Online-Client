// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ClientServerMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct ShootNormalProjectileAction : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static ShootNormalProjectileAction GetRootAsShootNormalProjectileAction(ByteBuffer _bb) { return GetRootAsShootNormalProjectileAction(_bb, new ShootNormalProjectileAction()); }
  public static ShootNormalProjectileAction GetRootAsShootNormalProjectileAction(ByteBuffer _bb, ShootNormalProjectileAction obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public ShootNormalProjectileAction __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public UdpMessages.ClientServerMessages.ShootNormalProjectileActionFix? Fix { get { int o = __p.__offset(4); return o != 0 ? (UdpMessages.ClientServerMessages.ShootNormalProjectileActionFix?)(new UdpMessages.ClientServerMessages.ShootNormalProjectileActionFix()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartShootNormalProjectileAction(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddFix(FlatBufferBuilder builder, Offset<UdpMessages.ClientServerMessages.ShootNormalProjectileActionFix> fixOffset) { builder.AddStruct(0, fixOffset.Value, 0); }
  public static Offset<UdpMessages.ClientServerMessages.ShootNormalProjectileAction> EndShootNormalProjectileAction(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    builder.Required(o, 4);  // fix
    return new Offset<UdpMessages.ClientServerMessages.ShootNormalProjectileAction>(o);
  }
};


}
