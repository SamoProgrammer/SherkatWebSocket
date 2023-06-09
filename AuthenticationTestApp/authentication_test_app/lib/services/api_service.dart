import 'package:http/http.dart' as http;
import 'package:unique_identifier/unique_identifier.dart';

class ApiService {
  Future<bool> authenticate(String token) async {
    String? identifier = await UniqueIdentifier.serial;
    var url = Uri.http('http://localhost:5034', 'api/Devices/Register');
    var response = await http.post(url, body: {
      'deviceId': identifier,
      'location': 'mashhad'
    }, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token',
    });
    print('Response status: ${response.statusCode}');
    print('Response body: ${response.body}');
    if (response.statusCode == 200) {
      return true;
    } else {
      return false;
    }
  }
}
