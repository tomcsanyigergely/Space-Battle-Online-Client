// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ServerClientMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct MatchCancelledMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static MatchCancelledMessage GetRootAsMatchCancelledMessage(ByteBuffer _bb) { return GetRootAsMatchCancelledMessage(_bb, new MatchCancelledMessage()); }
  public static MatchCancelledMessage GetRootAsMatchCancelledMessage(ByteBuffer _bb, MatchCancelledMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public MatchCancelledMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }


  public static void StartMatchCancelledMessage(FlatBufferBuilder builder) { builder.StartTable(0); }
  public static Offset<UdpMessages.ServerClientMessages.MatchCancelledMessage> EndMatchCancelledMessage(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<UdpMessages.ServerClientMessages.MatchCancelledMessage>(o);
  }
};


}
