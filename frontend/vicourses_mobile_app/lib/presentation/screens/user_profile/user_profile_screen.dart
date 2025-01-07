import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:flutter_html/flutter_html.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:url_launcher/url_launcher.dart';
import 'package:vicourses_mobile_app/models/user.dart';
import 'package:vicourses_mobile_app/presentation/screens/user_profile/cubit/user_profile.dart';
import 'package:vicourses_mobile_app/services/api/user_service.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class UserProfileScreen extends StatefulWidget {
  final String userId;

  const UserProfileScreen({
    super.key,
    required this.userId,
  });

  @override
  State<UserProfileScreen> createState() => _UserProfileScreenState();
}

class _UserProfileScreenState extends State<UserProfileScreen> {
  @override
  Widget build(BuildContext context) {
    return BlocProvider<UserProfileCubit>(
      create: (_) =>
          UserProfileCubit(UserService())..fetchProfile(widget.userId),
      child: Scaffold(
        appBar: AppBar(
          title: BlocBuilder<UserProfileCubit, UserProfileState>(
            builder: (context, state) {
              return Text(state.profile?.name ?? '');
            },
          ),
        ),
        body: BlocBuilder<UserProfileCubit, UserProfileState>(
          builder: (context, state) {
            if (state.isLoading) {
              return const Center(
                child: CircularProgressIndicator(),
              );
            }

            if (state.profile == null) {
              return Center(
                child: Text(AppLocalizations.of(context)!.noResults),
              );
            }

            return SingleChildScrollView(
              padding: const EdgeInsets.all(10),
              child: Column(
                children: [
                  _profileSection(context, state.profile!),
                ],
              ),
            );
          },
        ),
      ),
    );
  }

  Widget _profileSection(BuildContext context, PublicProfile profile) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            SizedBox(
              width: 100,
              height: 100,
              child: ClipOval(
                child: profile.thumbnailUrl != null
                    ? Image.network(
                        profile.thumbnailUrl!,
                        fit: BoxFit.cover,
                      )
                    : Image.asset(AppConstants.defaultUserImagePath),
              ),
            ),
            const SizedBox(width: 10),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisAlignment: MainAxisAlignment.start,
                children: [
                  Text(
                    profile.role.toUpperCase(),
                    style: const TextStyle(
                      fontSize: 12,
                      color: Colors.grey,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  Text(
                    profile.name,
                    style: const TextStyle(
                      fontSize: 22,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  Text(
                    profile.headline ?? '',
                    style: const TextStyle(
                      fontSize: 18,
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
        const SizedBox(height: 20),
        Column(
          children: [
            Text(
              AppLocalizations.of(context)!.totalStudents,
              style: const TextStyle(fontWeight: FontWeight.bold),
            ),
            Text(
              '${profile.totalEnrollmentCount}',
              style: const TextStyle(
                fontWeight: FontWeight.bold,
                fontSize: 18,
              ),
            ),
          ],
        ),
        const SizedBox(height: 20),
        if (profile.websiteUrl != null)
          ListTile(
            leading: const Icon(Icons.link, size: 30),
            title: const Text('Website'),
            trailing: const Icon(Icons.arrow_forward_ios),
            contentPadding: EdgeInsets.zero,
            onTap: () {
              launchUrl(Uri.parse(profile.websiteUrl!));
            },
          ),
        if (profile.youtubeUrl != null)
          ListTile(
            leading: SvgPicture.asset('assets/svg/youtube.svg', width: 30),
            title: const Text('Youtube'),
            trailing: const Icon(Icons.arrow_forward_ios),
            contentPadding: EdgeInsets.zero,
            onTap: () {
              launchUrl(Uri.parse(profile.youtubeUrl!));
            },
          ),
        if (profile.facebookUrl != null)
          ListTile(
            leading: SvgPicture.asset('assets/svg/facebook.svg', width: 30),
            title: const Text('Facebook'),
            trailing: const Icon(Icons.arrow_forward_ios),
            contentPadding: EdgeInsets.zero,
            onTap: () {
              launchUrl(Uri.parse(profile.facebookUrl!));
            },
          ),
        if (profile.linkedInUrl != null)
          ListTile(
            leading: SvgPicture.asset('assets/svg/linkedin.svg', width: 30),
            title: const Text('LinkedIn'),
            trailing: const Icon(Icons.arrow_forward_ios),
            contentPadding: EdgeInsets.zero,
            onTap: () {
              launchUrl(Uri.parse(profile.linkedInUrl!));
            },
          ),
        const SizedBox(height: 20),
        if (profile.description != null)
          Text(
            AppLocalizations.of(context)!.aboutMe,
            style: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 18,
            ),
          ),
        if (profile.description != null) Html(data: profile.description),
      ],
    );
  }
}
