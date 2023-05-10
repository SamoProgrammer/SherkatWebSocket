import 'package:flutter/material.dart';
import 'package:web_socket_channel/web_socket_channel.dart';

class WebSocketPage extends StatefulWidget {
  final String url;

  WebSocketPage({required this.url});

  @override
  _WebSocketPageState createState() => _WebSocketPageState();
}

class _WebSocketPageState extends State<WebSocketPage> {
  late WebSocketChannel _channel;
  final _controller = TextEditingController();
  List<String> _messages = [];

  @override
  void initState() {
    super.initState();
    _connect();
  }

  void _connect() {
    _channel = WebSocketChannel.connect(Uri.parse(widget.url));
    _channel.stream.listen((message) {
      setState(() {
        _messages.add(message);
      });
    });
  }

  void _disconnect() {
    _channel.sink.close();
  }

  void _sendMessage(String message) {
    _channel.sink.add(message);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('WebSocket Demo'),
      ),
      body: Column(
        children: [
          Expanded(
            child: ListView.builder(
              itemCount: _messages.length,
              itemBuilder: (context, index) {
                return ListTile(
                  title: Text(_messages[index]),
                );
              },
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: TextField(
              controller: _controller,
              decoration: const InputDecoration(
                border: OutlineInputBorder(),
                labelText: 'Enter a message',
              ),
            ),
          ),
          ButtonBar(
            children: [
              ElevatedButton(
                onPressed: _disconnect,
                child: const Text('Disconnect'),
              ),
              ElevatedButton(
                onPressed: () => _sendMessage(_controller.text),
                child: const Text('Send'),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
