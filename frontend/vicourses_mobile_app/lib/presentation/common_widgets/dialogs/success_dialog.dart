import 'package:flutter/material.dart';

Future<void> showSuccessDialog({
  required BuildContext context,
  required String text,
}) {
  return showDialog<void>(
    context: context,
    builder: (context) {
      return AlertDialog(
        title: const Text(
          'Success!',
          style: TextStyle(color: Colors.cyanAccent),
        ),
        content: Text(text),
        actions: [
          TextButton(
            onPressed: () {
              Navigator.of(context).pop();
            },
            child: const Text('OK'),
          ),
        ],
      );
    },
  );
}
