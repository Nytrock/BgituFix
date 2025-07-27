using EvtSource;
using System;
using System.Net.Http;
using UnityEngine;

public class SSEError {
    private EventSourceReader _reader;

    public EventSourceReader Reader => _reader;

    public SSEError(string uriPath) {
        Uri uri = new(uriPath);
        HttpClient client = new();
        client.SetToken();
        client.SetHeaders();

        _reader = new EventSourceReader(uri, client).Start();
        _reader.Disconnected += Reconnect;
    }

    private void Reconnect(object sender, DisconnectEventArgs args) {
        if (!Application.isPlaying)
            return;

        if (args.Exception.Message.Contains("Not Found")) {
            Debug.Log($"Ошибка, SSE сервис не найден");
        } else {
            Debug.Log($"Переподключение: {args.ReconnectDelay} - Ошибка: {args.Exception}");
            _reader.Start();
        }
    }

    public void Dispose() {
        _reader.Disconnected -= Reconnect;
        _reader.Dispose();
    }
}
