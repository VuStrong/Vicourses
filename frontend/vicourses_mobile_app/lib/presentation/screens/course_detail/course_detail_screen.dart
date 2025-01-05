import 'package:flutter/material.dart';

class CourseDetailScreen extends StatefulWidget {
  final String id;

  const CourseDetailScreen({
    super.key,
    required this.id,
  });

  @override
  State<CourseDetailScreen> createState() => _CourseDetailScreenState();
}

class _CourseDetailScreenState extends State<CourseDetailScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Course Detail ${widget.id}'),
      ),
      body: const SizedBox(),
    );
  }
}
