// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ServerClientMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct SpaceshipRespawnedMessageFix : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public SpaceshipRespawnedMessageFix __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public byte Owner { get { return __p.bb.Get(__p.bb_pos + 0); } }

  public static Offset<UdpMessages.ServerClientMessages.SpaceshipRespawnedMessageFix> CreateSpaceshipRespawnedMessageFix(FlatBufferBuilder builder, byte Owner) {
    builder.Prep(1, 1);
    builder.PutByte(Owner);
    return new Offset<UdpMessages.ServerClientMessages.SpaceshipRespawnedMessageFix>(builder.Offset);
  }
};


}
