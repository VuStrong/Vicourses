import 'package:flutter/material.dart';

enum SnackBarType { normal, success, error }

void showSnackBar({
  required BuildContext context,
  required String text,
  int duration = 2000,
  bool showCloseIcon = true,
  SnackBarType type = SnackBarType.normal,
}) {
  Color? bgColor;

  if (type == SnackBarType.success) {
    bgColor = Colors.green;
  } else if (type == SnackBarType.error) {
    bgColor = Colors.redAccent;
  }

  ScaffoldMessenger.of(context).showSnackBar(
    SnackBar(
      content: Text(
        text,
        style: const TextStyle(color: Colors.white),
      ),
      duration: Duration(milliseconds: duration),
      showCloseIcon: showCloseIcon,
      backgroundColor: bgColor,
    ),
  );
}
