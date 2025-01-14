import 'package:flutter/material.dart';

class BulletTextList extends StatelessWidget {
  final List<String> texts;
  final double fontSize;

  const BulletTextList({
    super.key,
    required this.texts,
    this.fontSize = 16,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      children: texts
          .map((text) => Padding(
                padding: const EdgeInsets.only(bottom: 5),
                child: _item(text),
              ))
          .toList(),
    );
  }

  Widget _item(String text) {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text(
          "•",
          style: TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 22,
          ),
        ),
        const SizedBox(width: 10),
        Expanded(
          child: Text(
            text,
            style: TextStyle(
              fontSize: fontSize,
            ),
          ),
        ),
      ],
    );
  }
}
