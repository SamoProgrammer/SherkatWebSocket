import 'package:authentication_test_app/services/api_service.dart';
import 'package:flutter/material.dart';

class MainPage extends StatefulWidget {
  const MainPage({super.key});

  @override
  State<MainPage> createState() => _MainPageState();
}

class _MainPageState extends State<MainPage> {
  final ApiService _apiService = ApiService();
  TextEditingController txtToken = TextEditingController();
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Column(
        children: [
          TextField(
            controller: txtToken,
          ),
          const Padding(padding: EdgeInsets.only(top: 20, bottom: 20)),
          TextButton(
            child: const Text('test server'),
            onPressed: () async {
              bool result = await _apiService.authenticate(txtToken.text);
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
        ],
      ),
    );
  }
}
