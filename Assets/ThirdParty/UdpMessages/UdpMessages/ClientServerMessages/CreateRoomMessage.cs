// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ClientServerMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct CreateRoomMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static CreateRoomMessage GetRootAsCreateRoomMessage(ByteBuffer _bb) { return GetRootAsCreateRoomMessage(_bb, new CreateRoomMessage()); }
  public static CreateRoomMessage GetRootAsCreateRoomMessage(ByteBuffer _bb, CreateRoomMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public CreateRoomMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public byte TeamSize { get { int o = __p.__offset(4); return o != 0 ? __p.bb.Get(o + __p.bb_pos) : (byte)0; } }
  public string Username { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetUsernameBytes() { return __p.__vector_as_span<byte>(6, 1); }
#else
  public ArraySegment<byte>? GetUsernameBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetUsernameArray() { return __p.__vector_as_array<byte>(6); }

  public static Offset<UdpMessages.ClientServerMessages.CreateRoomMessage> CreateCreateRoomMessage(FlatBufferBuilder builder,
      byte teamSize = 0,
      StringOffset usernameOffset = default(StringOffset)) {
    builder.StartTable(2);
    CreateRoomMessage.AddUsername(builder, usernameOffset);
    CreateRoomMessage.AddTeamSize(builder, teamSize);
    return CreateRoomMessage.EndCreateRoomMessage(builder);
  }

  public static void StartCreateRoomMessage(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddTeamSize(FlatBufferBuilder builder, byte teamSize) { builder.AddByte(0, teamSize, 0); }
  public static void AddUsername(FlatBufferBuilder builder, StringOffset usernameOffset) { builder.AddOffset(1, usernameOffset.Value, 0); }
  public static Offset<UdpMessages.ClientServerMessages.CreateRoomMessage> EndCreateRoomMessage(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    builder.Required(o, 6);  // username
    return new Offset<UdpMessages.ClientServerMessages.CreateRoomMessage>(o);
  }
};


}