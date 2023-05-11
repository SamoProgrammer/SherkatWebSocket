import 'package:flutter/material.dart';
import 'package:sherkat_websocket_app/pages/web_socket_page.dart';

void main() {
  runApp(const MainApp());
}

class MainApp extends StatelessWidget {
  const MainApp({super.key});

  @override
  Widget build(BuildContext context) {
    return const MaterialApp(
      home: WebSocketPage(url: 'ws://localhost:5034/notification',)
    );
  }
}
