import 'package:flutter/material.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:url_launcher/url_launcher.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/dialogs/prompt_dialog.dart';

class PayButton extends StatefulWidget {
  final String courseId;

  const PayButton({
    super.key,
    required this.courseId,
  });

  @override
  State<PayButton> createState() => _PayButtonState();
}

class _PayButtonState extends State<PayButton> {
  @override
  Widget build(BuildContext context) {
    final localizations = AppLocalizations.of(context)!;

    return ElevatedButton(
      style: ElevatedButton.styleFrom(
        backgroundColor: Theme.of(context).primaryColor,
        shape: const RoundedRectangleBorder(),
      ),
      onPressed: () async {
        final shouldRedirect = await showPromptDialog(
          context: context,
          prompt: localizations.paymentRedirectMessage,
          title: '',
        );

        if (shouldRedirect) {
          final domain = dotenv.env['WEB_CLIENT_URL'] ?? '';
          launchUrl(Uri.parse('$domain/checkout/course/${widget.courseId}'));
        }
      },
      child: Text(
        localizations.buyNow,
        style: const TextStyle(color: Colors.white),
      ),
    );
  }
}
