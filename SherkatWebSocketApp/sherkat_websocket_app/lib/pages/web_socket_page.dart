import 'package:flutter/material.dart';
import 'package:video_player/video_player.dart';
import 'package:web_socket_channel/web_socket_channel.dart';

class WebSocketPage extends StatefulWidget {
  final String url;

  const WebSocketPage({super.key, required this.url});

  @override
  _WebSocketPageState createState() => _WebSocketPageState();
}

class _WebSocketPageState extends State<WebSocketPage> {
  late WebSocketChannel _channel;
  List<String> _links = [];
  late VideoPlayerController _controller;
  int _currentVideoIndex = 0;

  @override
  void initState() {
    super.initState();
    _controller = VideoPlayerController.network(_links[_currentVideoIndex])
      ..initialize().then((_) {
        setState(() {
          _controller.play();
        });
      });
    _controller.addListener(_videoPlayerListener);
    _channel.stream.listen((message) {
      setState(() {
        // Handle the received video link
        addVideoLink(message);
      });
    });
  }

  void addVideoLink(String videoLink) {
    // Add the received video link to the videoList
    _links.add(videoLink);

    // If this is the first video link received, start playing it
    if (_currentVideoIndex == 0) {
      _controller = VideoPlayerController.network(_links[_currentVideoIndex])
        ..initialize().then((_) {
          setState(() {
            _controller.play();
          });
        });
      _controller.addListener(_videoPlayerListener);
    }
  }

  void _videoPlayerListener() {
    if (_controller.value.position >= _controller.value.duration) {
      setState(() {
        _currentVideoIndex++;
        if (_currentVideoIndex < _links.length) {
          _controller =
              VideoPlayerController.network(_links[_currentVideoIndex])
                ..initialize().then((_) {
                  setState(() {
                    // _initializeVideoPlayerFuture = _controller.initialize();
                    _controller.play();
                  });
                });
          _controller.addListener(_videoPlayerListener);
        } else {
          _currentVideoIndex = 0;
          _controller =
              VideoPlayerController.network(_links[_currentVideoIndex])
                ..initialize().then((_) {
                  setState(() {
                    // _initializeVideoPlayerFuture = _controller.initialize();
                    _controller.play();
                  });
                });
          _controller.addListener(_videoPlayerListener);
        }
      });
    }
  }

  void _disconnect() {
    _channel.sink.close();
  }

  @override
  void dispose() {
    // TODO: implement dispose
    _controller.dispose();
    _channel.sink.close(); 
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: ListView.builder(
        itemCount: _links.length,
        itemBuilder: (context, index) {
          return _buildVideoPlayer(index);
        },
      ),
      // floatingActionButton: FloatingActionButton(
      //   onPressed: () {
      //     setState(() {
      //       if (_controller.value.isPlaying) {
      //         _controller.pause();
      //       } else {
      //         _controller.play();
      //       }
      //     });
      //   },
      //   child: Icon(
      //     _controller.value.isPlaying ? Icons.pause : Icons.play_arrow,
      //   ),
      // ),
    );
  }

  Widget _buildVideoPlayer(int index) {
    if (index == _currentVideoIndex) {
      // return FutureBuilder(
      //   future: _initializeVideoPlayerFuture,
      //   builder: (context, snapshot) {
      //     if (snapshot.connectionState == ConnectionState.done) {
      return AspectRatio(
        aspectRatio: _controller.value.aspectRatio,
        child: VideoPlayer(_controller),
      );
      //     } else {
      //       return const Center(
      //         child: CircularProgressIndicator(),
      //       );
      //     }
      //   },
      // );
    } else {
      return Container(); // Placeholder widget for non-active videos
    }
  }
}
