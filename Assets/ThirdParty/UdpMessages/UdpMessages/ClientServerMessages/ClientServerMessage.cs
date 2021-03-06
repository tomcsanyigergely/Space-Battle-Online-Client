// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ClientServerMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct ClientServerMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static ClientServerMessage GetRootAsClientServerMessage(ByteBuffer _bb) { return GetRootAsClientServerMessage(_bb, new ClientServerMessage()); }
  public static ClientServerMessage GetRootAsClientServerMessage(ByteBuffer _bb, ClientServerMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public ClientServerMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public UdpMessages.ClientServerMessages.ClientServerMessageContent ContentType { get { int o = __p.__offset(4); return o != 0 ? (UdpMessages.ClientServerMessages.ClientServerMessageContent)__p.bb.Get(o + __p.bb_pos) : UdpMessages.ClientServerMessages.ClientServerMessageContent.NONE; } }
  public TTable? Content<TTable>() where TTable : struct, IFlatbufferObject { int o = __p.__offset(6); return o != 0 ? (TTable?)__p.__union<TTable>(o + __p.bb_pos) : null; }

  public static Offset<UdpMessages.ClientServerMessages.ClientServerMessage> CreateClientServerMessage(FlatBufferBuilder builder,
      UdpMessages.ClientServerMessages.ClientServerMessageContent content_type = UdpMessages.ClientServerMessages.ClientServerMessageContent.NONE,
      int contentOffset = 0) {
    builder.StartTable(2);
    ClientServerMessage.AddContent(builder, contentOffset);
    ClientServerMessage.AddContentType(builder, content_type);
    return ClientServerMessage.EndClientServerMessage(builder);
  }

  public static void StartClientServerMessage(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddContentType(FlatBufferBuilder builder, UdpMessages.ClientServerMessages.ClientServerMessageContent contentType) { builder.AddByte(0, (byte)contentType, 0); }
  public static void AddContent(FlatBufferBuilder builder, int contentOffset) { builder.AddOffset(1, contentOffset, 0); }
  public static Offset<UdpMessages.ClientServerMessages.ClientServerMessage> EndClientServerMessage(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    builder.Required(o, 6);  // content
    return new Offset<UdpMessages.ClientServerMessages.ClientServerMessage>(o);
  }
  public static void FinishClientServerMessageBuffer(FlatBufferBuilder builder, Offset<UdpMessages.ClientServerMessages.ClientServerMessage> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixedClientServerMessageBuffer(FlatBufferBuilder builder, Offset<UdpMessages.ClientServerMessages.ClientServerMessage> offset) { builder.FinishSizePrefixed(offset.Value); }
};


}
