// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ServerClientMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct MatchFinishedMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static MatchFinishedMessage GetRootAsMatchFinishedMessage(ByteBuffer _bb) { return GetRootAsMatchFinishedMessage(_bb, new MatchFinishedMessage()); }
  public static MatchFinishedMessage GetRootAsMatchFinishedMessage(ByteBuffer _bb, MatchFinishedMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public MatchFinishedMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public UdpMessages.Utilities.UInt8Struct? Winner { get { int o = __p.__offset(4); return o != 0 ? (UdpMessages.Utilities.UInt8Struct?)(new UdpMessages.Utilities.UInt8Struct()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartMatchFinishedMessage(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddWinner(FlatBufferBuilder builder, Offset<UdpMessages.Utilities.UInt8Struct> winnerOffset) { builder.AddStruct(0, winnerOffset.Value, 0); }
  public static Offset<UdpMessages.ServerClientMessages.MatchFinishedMessage> EndMatchFinishedMessage(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<UdpMessages.ServerClientMessages.MatchFinishedMessage>(o);
  }
};


}
