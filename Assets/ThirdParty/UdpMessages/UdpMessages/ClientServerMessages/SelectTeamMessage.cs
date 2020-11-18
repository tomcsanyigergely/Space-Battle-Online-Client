// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ClientServerMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct SelectTeamMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static SelectTeamMessage GetRootAsSelectTeamMessage(ByteBuffer _bb) { return GetRootAsSelectTeamMessage(_bb, new SelectTeamMessage()); }
  public static SelectTeamMessage GetRootAsSelectTeamMessage(ByteBuffer _bb, SelectTeamMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public SelectTeamMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public UdpMessages.Utilities.UInt8Struct? SelectedTeam { get { int o = __p.__offset(4); return o != 0 ? (UdpMessages.Utilities.UInt8Struct?)(new UdpMessages.Utilities.UInt8Struct()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartSelectTeamMessage(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddSelectedTeam(FlatBufferBuilder builder, Offset<UdpMessages.Utilities.UInt8Struct> selectedTeamOffset) { builder.AddStruct(0, selectedTeamOffset.Value, 0); }
  public static Offset<UdpMessages.ClientServerMessages.SelectTeamMessage> EndSelectTeamMessage(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<UdpMessages.ClientServerMessages.SelectTeamMessage>(o);
  }
};


}