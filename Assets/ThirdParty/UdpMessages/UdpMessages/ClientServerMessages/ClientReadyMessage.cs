// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ClientServerMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct ClientReadyMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static ClientReadyMessage GetRootAsClientReadyMessage(ByteBuffer _bb) { return GetRootAsClientReadyMessage(_bb, new ClientReadyMessage()); }
  public static ClientReadyMessage GetRootAsClientReadyMessage(ByteBuffer _bb, ClientReadyMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public ClientReadyMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }


  public static void StartClientReadyMessage(FlatBufferBuilder builder) { builder.StartTable(0); }
  public static Offset<UdpMessages.ClientServerMessages.ClientReadyMessage> EndClientReadyMessage(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<UdpMessages.ClientServerMessages.ClientReadyMessage>(o);
  }
};


}
