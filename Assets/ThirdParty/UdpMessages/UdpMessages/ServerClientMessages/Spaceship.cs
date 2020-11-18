// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ServerClientMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct Spaceship : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static Spaceship GetRootAsSpaceship(ByteBuffer _bb) { return GetRootAsSpaceship(_bb, new Spaceship()); }
  public static Spaceship GetRootAsSpaceship(ByteBuffer _bb, Spaceship obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public Spaceship __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public UdpMessages.ServerClientMessages.SpaceshipState? State { get { int o = __p.__offset(4); return o != 0 ? (UdpMessages.ServerClientMessages.SpaceshipState?)(new UdpMessages.ServerClientMessages.SpaceshipState()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public string Username { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetUsernameBytes() { return __p.__vector_as_span<byte>(6, 1); }
#else
  public ArraySegment<byte>? GetUsernameBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetUsernameArray() { return __p.__vector_as_array<byte>(6); }
  public bool IsConnected { get { int o = __p.__offset(8); return o != 0 ? 0!=__p.bb.Get(o + __p.bb_pos) : (bool)false; } }

  public static void StartSpaceship(FlatBufferBuilder builder) { builder.StartTable(3); }
  public static void AddState(FlatBufferBuilder builder, Offset<UdpMessages.ServerClientMessages.SpaceshipState> stateOffset) { builder.AddStruct(0, stateOffset.Value, 0); }
  public static void AddUsername(FlatBufferBuilder builder, StringOffset usernameOffset) { builder.AddOffset(1, usernameOffset.Value, 0); }
  public static void AddIsConnected(FlatBufferBuilder builder, bool isConnected) { builder.AddBool(2, isConnected, false); }
  public static Offset<UdpMessages.ServerClientMessages.Spaceship> EndSpaceship(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    builder.Required(o, 6);  // username
    return new Offset<UdpMessages.ServerClientMessages.Spaceship>(o);
  }
};


}