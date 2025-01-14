import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:flutter_html/flutter_html.dart';
import 'package:vicourses_mobile_app/models/quiz.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/snack_bar.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/cubit/doing_quizzes.dart';

class DoingQuizzesScreen extends StatefulWidget {
  final List<Quiz> quizzes;

  const DoingQuizzesScreen({
    super.key,
    required this.quizzes,
  });

  @override
  State<DoingQuizzesScreen> createState() => _DoingQuizzesScreenState();
}

class _DoingQuizzesScreenState extends State<DoingQuizzesScreen> {
  @override
  Widget build(BuildContext context) {
    return BlocProvider<DoingQuizzesCubit>(
      create: (_) => DoingQuizzesCubit(quizzes: widget.quizzes),
      child: Scaffold(
        appBar: AppBar(
          automaticallyImplyLeading: false,
          actions: [
            IconButton(
              onPressed: () => Navigator.of(context).pop(),
              icon: const Icon(Icons.close),
            ),
          ],
        ),
        body: _quizzesBuilder(),
        floatingActionButton: _submitButton(),
      ),
    );
  }

  Widget _submitButton() {
    return BlocBuilder<DoingQuizzesCubit, DoingQuizzesState>(
      builder: (context, state) {
        if (state.result != null) {
          return ElevatedButton(
            onPressed: () {
              context.read<DoingQuizzesCubit>().reset();
            },
            child: Text(
              AppLocalizations.of(context)!.tryAgain,
            ),
          );
        }

        return ElevatedButton(
          onPressed: () {
            if (state.canSubmit) {
              context.read<DoingQuizzesCubit>().submit();
            } else {
              showSnackBar(
                context: context,
                text: AppLocalizations.of(context)!.notCompletedQuizzesMessage,
              );
            }
          },
          style: ElevatedButton.styleFrom(
            backgroundColor: Colors.black,
          ),
          child: Text(
            AppLocalizations.of(context)!.check,
            style: const TextStyle(
              color: Colors.white,
            ),
          ),
        );
      },
    );
  }

  Widget _quizzesBuilder() {
    return BlocBuilder<DoingQuizzesCubit, DoingQuizzesState>(
      builder: (context, state) {
        if (state.quizzes.isEmpty) {
          return Center(
            child: Text(AppLocalizations.of(context)!.noResults),
          );
        }

        if (state.result != null) {
          return _ResultView(result: state.result!);
        }

        return ListView(
          padding: const EdgeInsets.all(10),
          children: state.quizzes.map((quiz) {
            return Padding(
              padding: const EdgeInsets.only(bottom: 20),
              child: _QuizItem(
                quiz: quiz,
                selectedAnswers: state.answers[quiz.number] ?? [],
              ),
            );
          }).toList(),
        );
      },
    );
  }
}

class _ResultView extends StatelessWidget {
  final Result result;

  const _ResultView({required this.result});

  @override
  Widget build(BuildContext context) {
    final correctQuizzesLength = result.correctQuizzes.length;
    final totalQuizzes = correctQuizzesLength + result.inCorrectQuizzes.length;
    final localizations = AppLocalizations.of(context)!;

    return ListView(
      padding: const EdgeInsets.all(10),
      children: [
        Text(
          result.allCorrect
              ? localizations.answerAllQuizzesCorrect
              : localizations.answerCorrectQuizzes(
                  correctQuizzesLength, totalQuizzes),
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 20,
          ),
        ),
        ...result.correctQuizzes.map((quiz) {
          return ListTile(
            contentPadding: EdgeInsets.zero,
            leading: const Icon(
              Icons.check,
              color: Colors.greenAccent,
            ),
            title: Text(localizations.quizNumber(quiz.number)),
          );
        }),
        ...result.inCorrectQuizzes.map((quiz) {
          return ListTile(
            contentPadding: EdgeInsets.zero,
            leading: const Icon(
              Icons.close,
              color: Colors.redAccent,
            ),
            title: Text(localizations.quizNumber(quiz.number)),
          );
        }),
      ],
    );
  }
}

class _QuizItem extends StatelessWidget {
  final Quiz quiz;
  final List<int> selectedAnswers;

  const _QuizItem({
    required this.quiz,
    required this.selectedAnswers,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          '${AppLocalizations.of(context)!.quizNumber(quiz.number)}:',
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 16,
          ),
        ),
        Html(
          data: quiz.title,
          style: {
            'body': Style(
              margin: Margins.all(0),
              padding: HtmlPaddings.zero,
              fontSize: FontSize.large,
            ),
            "p": Style(
              padding: HtmlPaddings.zero,
              margin: Margins.zero,
            ),
          },
        ),
        const SizedBox(height: 10),
        ...quiz.answers.map((answer) {
          return _AnswerItem(
            answer: answer,
            selected: selectedAnswers.contains(answer.number),
            onTap: () {
              context
                  .read<DoingQuizzesCubit>()
                  .setQuizAnswer(quiz.number, answer.number);
            },
          );
        }),
      ],
    );
  }
}

class _AnswerItem extends StatelessWidget {
  final Answer answer;
  final bool selected;
  final void Function() onTap;

  const _AnswerItem({
    required this.answer,
    required this.selected,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 5),
      child: ListTile(
        title: Text(answer.title),
        contentPadding: EdgeInsets.zero,
        shape: Border.all(),
        leading: Radio<int>(
          value: answer.number,
          groupValue: selected ? answer.number : -1,
          onChanged: (_) => onTap(),
        ),
        onTap: onTap,
      ),
    );
  }
}
