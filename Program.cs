global using System.Diagnostics;
global using NetCraft;
global using NetCraft.Models;
using System;
using OpenTK;

var window = new Window(new() { UpdateFrequency = 120, }, new() { ClientSize = (960, 540) });

// 创建WebSocket客户端
var webSocketClient = new WebSocketClient();
webSocketClient.MessageReceived += (sender, message) =>
{
    // 将收到的消息发送到窗口
    window.OnWebSocketMessageReceived(message);
};

// 开始WebSocket客户端
_ = Task.Run(() => webSocketClient.StartAsync());

window.Run();
