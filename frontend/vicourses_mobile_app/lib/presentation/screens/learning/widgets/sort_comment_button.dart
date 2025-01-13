import 'package:flutter/material.dart';

class SortCommentButton extends StatelessWidget {
  final String text;
  final bool active;
  final void Function() onPressed;

  const SortCommentButton({
    required this.text,
    required this.active,
    required this.onPressed,
  });

  @override
  Widget build(BuildContext context) {
    return OutlinedButton(
      onPressed: onPressed,
      style: OutlinedButton.styleFrom(
        backgroundColor: active ? Theme.of(context).primaryColor : null,
      ),
      child: Text(
        text,
        style: TextStyle(
          color: active ? Colors.white : Colors.black,
        ),
      ),
    );
  }
}