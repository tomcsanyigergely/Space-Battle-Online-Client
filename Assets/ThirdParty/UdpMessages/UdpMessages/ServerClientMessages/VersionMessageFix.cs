// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace UdpMessages.ServerClientMessages
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct VersionMessageFix : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public VersionMessageFix __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public UdpMessages.ServerClientMessages.Version MinVersion { get { return (new UdpMessages.ServerClientMessages.Version()).__assign(__p.bb_pos + 0, __p.bb); } }
  public UdpMessages.ServerClientMessages.Version MaxVersion { get { return (new UdpMessages.ServerClientMessages.Version()).__assign(__p.bb_pos + 3, __p.bb); } }
  public bool NewMatchesAllowed { get { return 0!=__p.bb.Get(__p.bb_pos + 6); } }

  public static Offset<UdpMessages.ServerClientMessages.VersionMessageFix> CreateVersionMessageFix(FlatBufferBuilder builder, byte minVersion_MajorVersion, byte minVersion_MinorVersion, byte minVersion_PatchVersion, byte maxVersion_MajorVersion, byte maxVersion_MinorVersion, byte maxVersion_PatchVersion, bool NewMatchesAllowed) {
    builder.Prep(1, 7);
    builder.PutBool(NewMatchesAllowed);
    builder.Prep(1, 3);
    builder.PutByte(maxVersion_PatchVersion);
    builder.PutByte(maxVersion_MinorVersion);
    builder.PutByte(maxVersion_MajorVersion);
    builder.Prep(1, 3);
    builder.PutByte(minVersion_PatchVersion);
    builder.PutByte(minVersion_MinorVersion);
    builder.PutByte(minVersion_MajorVersion);
    return new Offset<UdpMessages.ServerClientMessages.VersionMessageFix>(builder.Offset);
  }
};


}
