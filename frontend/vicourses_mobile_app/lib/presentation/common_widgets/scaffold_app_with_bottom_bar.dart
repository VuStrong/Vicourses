import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';

class ScaffoldAppWithBottomBar extends StatefulWidget {
  final Widget child;
  final String location;

  const ScaffoldAppWithBottomBar({
    super.key,
    required this.child,
    required this.location,
  });

  @override
  State<ScaffoldAppWithBottomBar> createState() =>
      _ScaffoldAppWithBottomBarState();
}

class _ScaffoldAppWithBottomBarState extends State<ScaffoldAppWithBottomBar> {
  int _currentIndex = 0;

  int _getCurrentIndex() {
    if (widget.location == AppRoutes.home) {
      return 0;
    } else if (widget.location == AppRoutes.search) {
      return 1;
    } else if (widget.location == AppRoutes.study) {
      return 2;
    } else if (widget.location == AppRoutes.wishlist) {
      return 3;
    } else if (widget.location == AppRoutes.accountOverview) {
      return 4;
    }

    return 0;
  }

  void _tapTab(BuildContext context, int index) {
    if (index == _currentIndex) return;

    setState(() {
      _currentIndex = index;
    });

    if (index == 0) {
      context.go(AppRoutes.home);
    } else if (index == 1) {
      context.go(AppRoutes.search);
    } else if (index == 2) {
      context.go(AppRoutes.study);
    } else if (index == 3) {
      context.go(AppRoutes.wishlist);
    } else if (index == 4) {
      context.go(AppRoutes.accountOverview);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(child: widget.child),
      bottomNavigationBar: BottomNavigationBar(
        type: BottomNavigationBarType.fixed,
        currentIndex: _getCurrentIndex(),
        onTap: (index) {
          _tapTab(context, index);
        },
        items: [
          BottomNavigationBarItem(
            icon: const Icon(Icons.home),
            label: AppLocalizations.of(context)!.home,
          ),
          BottomNavigationBarItem(
            icon: const Icon(Icons.search_outlined),
            label: AppLocalizations.of(context)!.search,
          ),
          BottomNavigationBarItem(
            icon: const Icon(Icons.play_circle_outline),
            label: AppLocalizations.of(context)!.study,
          ),
          BottomNavigationBarItem(
            icon: const Icon(Icons.favorite),
            label: AppLocalizations.of(context)!.wishlist,
          ),
          BottomNavigationBarItem(
            icon: const Icon(Icons.account_circle_rounded),
            label: AppLocalizations.of(context)!.account,
          ),
        ],
      ),
    );
  }
}
