// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ClientServerMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct ClientInputMessageFix : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public ClientInputMessageFix __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float InputX { get { return __p.bb.GetFloat(__p.bb_pos + 0); } }
  public float InputY { get { return __p.bb.GetFloat(__p.bb_pos + 4); } }
  public float Direction { get { return __p.bb.GetFloat(__p.bb_pos + 8); } }

  public static Offset<UdpMessages.ClientServerMessages.ClientInputMessageFix> CreateClientInputMessageFix(FlatBufferBuilder builder, float InputX, float InputY, float Direction) {
    builder.Prep(4, 12);
    builder.PutFloat(Direction);
    builder.PutFloat(InputY);
    builder.PutFloat(InputX);
    return new Offset<UdpMessages.ClientServerMessages.ClientInputMessageFix>(builder.Offset);
  }
};


}
