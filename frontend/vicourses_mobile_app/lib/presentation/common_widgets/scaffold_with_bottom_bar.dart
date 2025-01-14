import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

class ScaffoldWithBottomBar extends StatelessWidget {
  final StatefulNavigationShell navigationShell;

  const ScaffoldWithBottomBar({
    super.key,
    required this.navigationShell,
  });

  void _goBranch(int index) {
    navigationShell.goBranch(
      index,
      initialLocation: index == navigationShell.currentIndex,
    );
  }

  @override
  Widget build(BuildContext context) {
    return BackButtonListener(
      onBackButtonPressed: () async {
        if (context.canPop()) return false;

        if (navigationShell.currentIndex != 0) {
          navigationShell.goBranch(0);
          return true;
        }

        return false;
      },
      child: Scaffold(
        body: SafeArea(child: navigationShell),
        bottomNavigationBar: BottomNavigationBar(
          type: BottomNavigationBarType.fixed,
          currentIndex: navigationShell.currentIndex,
          onTap: _goBranch,
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
      ),
    );
  }
}
