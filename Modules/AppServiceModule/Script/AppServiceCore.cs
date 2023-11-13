using System;
using BestHTTP.WebSocket;

public interface IAppServiceModule
{
    void OnOpen(WebSocket ws);
    void OnMessageReceived(WebSocket ws, byte[] data);
    void OnMessageReceived(WebSocket ws, string message);
    void OnClosed(WebSocket ws, UInt16 code, string message);
    void OnError(WebSocket ws, Exception ex);
}