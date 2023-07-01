import 'package:authentication_test_app/services/api_service.dart';
import 'package:flutter/material.dart';

class MainPage extends StatefulWidget {
  const MainPage({super.key});

  @override
  State<MainPage> createState() => _MainPageState();
}

class _MainPageState extends State<MainPage> {
  final ApiService _apiService = ApiService();
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: TextButton(
        child: const Text('test server'),
        onPressed: () async {
          bool result = await _apiService.authenticate();
          String response = '';
          final snackBar = SnackBar(content: Text(response));
          if (result) {
            setState(() {
              response = "موفق";
            });
          } else {
            setState(() {
              response = "ناموفق";
            });
          }
          ScaffoldMessenger.of(context).showSnackBar(snackBar);
        },
      ),
    );
  }
}
