import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/edit_profile/cubit/edit_profile.dart';

class EditBasicInfoTab extends StatefulWidget {
  EditBasicInfoTab({super.key});

  final RegExp urlRegex = RegExp(
    r"^(https?:\/\/)?(www\.)?([a-zA-Z0-9-_]+\.)+[a-zA-Z]{2,}(:\d+)?(\/[^\s]*)?$",
    caseSensitive: false,
    multiLine: false,
  );

  @override
  State<EditBasicInfoTab> createState() => _EditBasicInfoTabState();
}

class _EditBasicInfoTabState extends State<EditBasicInfoTab>
    with AutomaticKeepAliveClientMixin<EditBasicInfoTab> {
  final _formKey = GlobalKey<FormState>();
  late final TextEditingController nameController;
  late final TextEditingController headlineController;
  late final TextEditingController websiteUrlController;
  late final TextEditingController youtubeUrlController;
  late final TextEditingController facebookUrlController;
  late final TextEditingController linkedInUrlController;

  @override
  void initState() {
    final user = BlocProvider.of<UserBloc>(context).state.user;

    nameController = TextEditingController(text: user?.name);
    headlineController = TextEditingController(text: user?.headline);
    websiteUrlController = TextEditingController(text: user?.websiteUrl);
    youtubeUrlController = TextEditingController(text: user?.youtubeUrl);
    facebookUrlController = TextEditingController(text: user?.facebookUrl);
    linkedInUrlController = TextEditingController(text: user?.linkedInUrl);

    super.initState();
  }

  @override
  void dispose() {
    nameController.dispose();
    headlineController.dispose();
    websiteUrlController.dispose();
    youtubeUrlController.dispose();
    facebookUrlController.dispose();
    linkedInUrlController.dispose();

    super.dispose();
  }

  @override
  bool get wantKeepAlive => true;

  @override
  Widget build(BuildContext context) {
    super.build(context);

    return Form(
      key: _formKey,
      child: ListView(
        padding: const EdgeInsets.all(20),
        children: [
          _nameField(context),
          _headlineField(context),
          const SizedBox(height: 40),
          Text(
            AppLocalizations.of(context)!.socialMedias,
            style: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 12,
            ),
          ),
          _websiteUrlField(context),
          _youtubeUrlField(context),
          _facebookUrlField(context),
          _linkedInUrlField(context),
          const SizedBox(height: 20),
          Row(
            children: [
              Expanded(
                child: _submitButton(context),
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _nameField(BuildContext context) {
    return TextFormField(
      controller: nameController,
      keyboardType: TextInputType.text,
      validator: (value) {
        String msg = AppLocalizations.of(context)!.textLength(2, 50);

        if (value?.isEmpty ?? true) return msg;

        return value!.length >= 2 && value.length <= 50 ? null : msg;
      },
      decoration: InputDecoration(
        labelText: AppLocalizations.of(context)!.username,
        floatingLabelBehavior: FloatingLabelBehavior.always,
      ),
      onTapOutside: (event) => FocusScope.of(context).unfocus(),
      style: const TextStyle(
        fontWeight: FontWeight.w500,
      ),
    );
  }

  Widget _headlineField(BuildContext context) {
    return TextFormField(
      controller: headlineController,
      keyboardType: TextInputType.text,
      validator: (value) {
        String msg = AppLocalizations.of(context)!.textMax(60);

        if (value != null && value.length > 60) return msg;

        return null;
      },
      decoration: const InputDecoration(
        labelText: 'Headline',
        floatingLabelBehavior: FloatingLabelBehavior.always,
      ),
      onTapOutside: (event) => FocusScope.of(context).unfocus(),
      style: const TextStyle(
        fontWeight: FontWeight.w500,
      ),
    );
  }

  Widget _websiteUrlField(BuildContext context) {
    return TextFormField(
      controller: websiteUrlController,
      keyboardType: TextInputType.url,
      validator: (value) {
        if (value == null || value.isEmpty) return null;

        if (!widget.urlRegex.hasMatch(value)) {
          return AppLocalizations.of(context)!.urlInvalid;
        }

        return null;
      },
      decoration: const InputDecoration(
        labelText: 'Website',
        hintText: 'http://example.com',
        icon: Icon(Icons.web, size: 40),
      ),
      onTapOutside: (event) => FocusScope.of(context).unfocus(),
      style: const TextStyle(
        fontWeight: FontWeight.w500,
      ),
    );
  }

  Widget _youtubeUrlField(BuildContext context) {
    return TextFormField(
      controller: youtubeUrlController,
      keyboardType: TextInputType.url,
      validator: (value) {
        if (value == null || value.isEmpty) return null;

        if (!widget.urlRegex.hasMatch(value)) {
          return AppLocalizations.of(context)!.urlInvalid;
        }

        return null;
      },
      decoration: InputDecoration(
        labelText: 'Youtube',
        hintText: 'https://www.youtube.com/username',
        icon: SvgPicture.asset('assets/svg/youtube.svg', width: 40),
      ),
      onTapOutside: (event) => FocusScope.of(context).unfocus(),
      style: const TextStyle(
        fontWeight: FontWeight.w500,
      ),
    );
  }

  Widget _facebookUrlField(BuildContext context) {
    return TextFormField(
      controller: facebookUrlController,
      keyboardType: TextInputType.url,
      validator: (value) {
        if (value == null || value.isEmpty) return null;

        if (!widget.urlRegex.hasMatch(value)) {
          return AppLocalizations.of(context)!.urlInvalid;
        }

        return null;
      },
      decoration: InputDecoration(
        labelText: 'Facebook',
        hintText: 'https://www.facebook.com/username',
        icon: SvgPicture.asset('assets/svg/facebook.svg', width: 40),
      ),
      onTapOutside: (event) => FocusScope.of(context).unfocus(),
      style: const TextStyle(
        fontWeight: FontWeight.w500,
      ),
    );
  }

  Widget _linkedInUrlField(BuildContext context) {
    return TextFormField(
      controller: linkedInUrlController,
      keyboardType: TextInputType.url,
      validator: (value) {
        if (value == null || value.isEmpty) return null;

        if (!widget.urlRegex.hasMatch(value)) {
          return AppLocalizations.of(context)!.urlInvalid;
        }

        return null;
      },
      decoration: InputDecoration(
        labelText: 'LinkedIn',
        hintText: 'http://www.linkedin.com/id',
        icon: SvgPicture.asset('assets/svg/linkedin.svg', width: 40),
      ),
      onTapOutside: (event) => FocusScope.of(context).unfocus(),
      style: const TextStyle(
        fontWeight: FontWeight.w500,
      ),
    );
  }

  Widget _submitButton(BuildContext context) {
    return ElevatedButton(
      onPressed: () {
        if (_formKey.currentState!.validate()) {
          context.read<EditProfileCubit>().editProfile(
            name: nameController.text,
            headline: headlineController.text,
            websiteUrl: websiteUrlController.text,
            youtubeUrl: youtubeUrlController.text,
            facebookUrl: facebookUrlController.text,
            linkedInUrl: linkedInUrlController.text,
          );
        }
      },
      style: ElevatedButton.styleFrom(
        backgroundColor: Theme.of(context).colorScheme.primary,
      ),
      child: Text(
        AppLocalizations.of(context)!.save,
        style: const TextStyle(color: Colors.white),
      ),
    );
  }
}
