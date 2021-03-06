// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ServerClientMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Collision : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static Collision GetRootAsCollision(ByteBuffer _bb) { return GetRootAsCollision(_bb, new Collision()); }
  public static Collision GetRootAsCollision(ByteBuffer _bb, Collision obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public Collision __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public UdpMessages.ServerClientMessages.CollisionFix? Fix { get { int o = __p.__offset(4); return o != 0 ? (UdpMessages.ServerClientMessages.CollisionFix?)(new UdpMessages.ServerClientMessages.CollisionFix()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartCollision(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddFix(FlatBufferBuilder builder, Offset<UdpMessages.ServerClientMessages.CollisionFix> fixOffset) { builder.AddStruct(0, fixOffset.Value, 0); }
  public static Offset<UdpMessages.ServerClientMessages.Collision> EndCollision(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<UdpMessages.ServerClientMessages.Collision>(o);
  }
};


}
