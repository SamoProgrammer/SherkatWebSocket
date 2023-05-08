import 'dart:convert';
import 'dart:io';
import 'package:http/http.dart' as http;

class ApiService {
  void main() async {
    // Create a WebSocket channel
    final channel = await http.Client()
        .get(Uri.parse('https://echo.websocket.org'))
        .then((response) {
      return WebSocket.connect(response.headers['location']!);
    });

    // Listen for WebSocket events
    channel.listen((event) {
      print('Received: $event');
    }, onError: (error) {
      print('Error: $error');
    }, onDone: () {
      print('Done');
    });

    // Send a WebSocket message using http package
    channel.add('Hello, WebSocket!');

    // Close the WebSocket channel
    channel.close();
  }
}

