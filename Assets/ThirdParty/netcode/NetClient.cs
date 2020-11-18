using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

class NetClient
{
    private static NetClient netClient;

    public delegate void OnConnectedDelegate();
    public delegate void OnDisconnectedDelegate();
    public delegate void OnReceivedDelegate(byte[] message, UInt16 size);

    private delegate void OnReceivedInnerDelegate(IntPtr message, UInt16 size);    

    private OnConnectedDelegate onConnectedCallback;
    private OnDisconnectedDelegate onDisconnectedCallback;
    private OnReceivedDelegate onReceivedCallback;

    public static bool Initialize()
    {
        return Net_Initialize();
    }

    public static void Deinitialize()
    {
        Net_Deinitialize();
    }

    private static void OnConnected() {
        netClient.onConnectedCallback();
    }

    private static void OnDisconnected() {
        netClient.onDisconnectedCallback();
    }

    private static void OnReceived(IntPtr message, UInt16 size)
    {
        byte[] msgArray = new byte[size];
        Marshal.Copy(message, msgArray, 0, msgArray.Length);

        netClient.onReceivedCallback(msgArray, size);
    }

    public NetClient(
        OnConnectedDelegate onConnectedCallback,
        OnDisconnectedDelegate onDisconnectedCallback,
        OnReceivedDelegate onReceivedCallback)
    {
        this.onConnectedCallback = onConnectedCallback;
        this.onDisconnectedCallback = onDisconnectedCallback;
        this.onReceivedCallback = onReceivedCallback;

        handle = NetClient_Create(OnConnected, OnDisconnected, OnReceived);
        netClient = this;
    }

    ~NetClient()
    {
        NetClient_Destroy(handle);
    }

    public bool Connect(string address, UInt16 port)
    {
        return NetClient_Connect(handle, address, port);
    }

    public bool SendReliable(byte[] message, UInt16 size)
    {
        return NetClient_SendReliable(handle, message, size);
    }

    public bool SendUnreliable(byte[] message, UInt16 size)
    {
        return NetClient_SendUnreliable(handle, message, size);
    }

    public void Disconnect()
    {
        NetClient_Disconnect(handle);
    }

    public int Listen(UInt32 timeout)
    {
        return NetClient_Listen(handle, timeout);
    }

    private IntPtr handle;

    [DllImport("libnetcode", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool Net_Initialize();

    [DllImport("libnetcode", CallingConvention = CallingConvention.Cdecl)]
    private static extern void Net_Deinitialize();

    [DllImport("libnetcode", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr NetClient_Create(
        OnConnectedDelegate onConnected,
        OnDisconnectedDelegate onDisconnected,
        OnReceivedInnerDelegate onReceivedInner);

    [DllImport("libnetcode", CallingConvention = CallingConvention.Cdecl)]
    private static extern void NetClient_Destroy(IntPtr handle);

    [DllImport("libnetcode", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool NetClient_Connect(IntPtr handle, string address, UInt16 port);

    [DllImport("libnetcode", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool NetClient_SendReliable(IntPtr handle, byte[] message, UInt16 size);

    [DllImport("libnetcode", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool NetClient_SendUnreliable(IntPtr handle, byte[] message, UInt16 size);

    [DllImport("libnetcode", CallingConvention = CallingConvention.Cdecl)]
    private static extern void NetClient_Disconnect(IntPtr handle);

    [DllImport("libnetcode", CallingConvention = CallingConvention.Cdecl)]
    private static extern int NetClient_Listen(IntPtr handle, UInt32 timeout);
}