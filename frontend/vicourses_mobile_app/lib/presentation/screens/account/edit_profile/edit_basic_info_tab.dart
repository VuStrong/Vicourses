import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/models/user.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';

class EditBasicInfoTab extends StatefulWidget {
  const EditBasicInfoTab({super.key});

  @override
  State<EditBasicInfoTab> createState() => _EditBasicInfoTabState();
}

class _EditBasicInfoTabState extends State<EditBasicInfoTab>
    with AutomaticKeepAliveClientMixin<EditBasicInfoTab> {
  final _formKey = GlobalKey<FormState>();
  late final TextEditingController nameController;
  late final TextEditingController headlineController;

  @override
  void initState() {
    final user = BlocProvider.of<UserBloc>(context).state.user;

    nameController = TextEditingController(text: user?.name);
    headlineController = TextEditingController(text: user?.headline);

    super.initState();
  }

  @override
  void dispose() {
    nameController.dispose();
    headlineController.dispose();

    super.dispose();
  }

  @override
  bool get wantKeepAlive => true;

  @override
  Widget build(BuildContext context) {
    super.build(context);

    return Form(
      key: _formKey,
      child: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            _nameField(context),
            _headlineField(context),
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
      ),
    );
  }

  Widget _nameField(BuildContext context) {
    return TextFormField(
      controller: nameController,
      keyboardType: TextInputType.text,
      textInputAction: TextInputAction.next,
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
      textInputAction: TextInputAction.next,
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

  Widget _submitButton(BuildContext context) {
    return ElevatedButton(
      onPressed: () {
        if (_formKey.currentState!.validate()) {
          //
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
