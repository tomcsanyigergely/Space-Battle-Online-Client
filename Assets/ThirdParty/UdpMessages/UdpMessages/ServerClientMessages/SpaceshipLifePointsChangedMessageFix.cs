// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ServerClientMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct SpaceshipLifePointsChangedMessageFix : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public SpaceshipLifePointsChangedMessageFix __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public byte Owner { get { return __p.bb.Get(__p.bb_pos + 0); } }
  public float LifePoints { get { return __p.bb.GetFloat(__p.bb_pos + 4); } }

  public static Offset<UdpMessages.ServerClientMessages.SpaceshipLifePointsChangedMessageFix> CreateSpaceshipLifePointsChangedMessageFix(FlatBufferBuilder builder, byte Owner, float LifePoints) {
    builder.Prep(4, 8);
    builder.PutFloat(LifePoints);
    builder.Pad(3);
    builder.PutByte(Owner);
    return new Offset<UdpMessages.ServerClientMessages.SpaceshipLifePointsChangedMessageFix>(builder.Offset);
  }
};


}
