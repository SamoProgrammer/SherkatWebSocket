import 'package:http/http.dart' as http;
import 'package:flutter_udid/flutter_udid.dart';

class ApiService {
  Future<bool> authenticate() async {
    String udid = await FlutterUdid.udid;
    var url = Uri.https('http://localhost:5034', 'devices/authenticate');
    var response = await http.post(url, body: {'androidId': udid});
    print('Response status: ${response.statusCode}');
    print('Response body: ${response.body}');
    return true;
  }
}
