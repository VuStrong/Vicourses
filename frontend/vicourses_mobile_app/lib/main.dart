import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:loader_overlay/loader_overlay.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/categories/categories.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/wishlist/wishlist.dart';
import 'package:vicourses_mobile_app/routes/app_go_router.dart';
import 'package:vicourses_mobile_app/services/api/auth_service.dart';
import 'package:vicourses_mobile_app/services/api/category_service.dart';
import 'package:vicourses_mobile_app/services/api/user_service.dart';
import 'package:vicourses_mobile_app/services/api/wishlist_service.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';
import 'package:vicourses_mobile_app/utils/local_storage.dart';

Future main() async {
  await dotenv.load(fileName: ".env");

  runApp(const VicoursesApp());
}

class VicoursesApp extends StatelessWidget {
  const VicoursesApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MultiBlocProvider(
      providers: [
        BlocProvider<UserBloc>(
          lazy: false,
          create: (_) => UserBloc(AuthService(), UserService(), LocalStorage()),
        ),
        BlocProvider<CategoriesCubit>(
          lazy: false,
          create: (_) => CategoriesCubit(CategoryService())..getCategories(),
        ),
        BlocProvider<WishlistCubit>(
          lazy: false,
          create: (_) => WishlistCubit(WishlistService())..getWishlist(),
        ),
      ],
      child: GlobalLoaderOverlay(
        overlayColor: Colors.black.withOpacity(0.8),
        child: BlocListener<UserBloc, UserState>(
          listenWhen: (prev, current) => prev.status != current.status,
          listener: (_, __) {
            AppGoRouter.router.refresh();
          },
          child: MaterialApp.router(
            title: AppConstants.appName,
            debugShowCheckedModeBanner: false,
            theme: ThemeData(
              colorScheme: ColorScheme.fromSeed(
                seedColor: const Color.fromRGBO(0, 113, 64, 1),
              ),
              useMaterial3: true,
              scaffoldBackgroundColor: Colors.white,
            ),
            localizationsDelegates: const [
              AppLocalizations.delegate,
              GlobalMaterialLocalizations.delegate,
              GlobalWidgetsLocalizations.delegate,
              GlobalCupertinoLocalizations.delegate,
            ],
            supportedLocales: const [
              Locale('en'),
              Locale('vi'),
            ],
            routerConfig: AppGoRouter.router,
          ),
        ),
      ),
    );
  }
}
