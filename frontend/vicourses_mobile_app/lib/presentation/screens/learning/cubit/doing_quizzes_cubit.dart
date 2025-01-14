import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/quiz.dart';

part 'doing_quizzes_state.dart';

class DoingQuizzesCubit extends Cubit<DoingQuizzesState> {
  DoingQuizzesCubit({
    required List<Quiz> quizzes,
  }) : super(DoingQuizzesState(quizzes: quizzes, answers: {}));

  void setQuizAnswer(int quizNumber, int answerNumber) {
    final quiz = state.quizzes.firstWhere((quiz) => quiz.number == quizNumber);

    final answers = state.answers[quizNumber] ?? [];

    if (answers.contains(answerNumber)) {
      answers.remove(answerNumber);
    } else {
      if (!quiz.isMultiChoice) {
        answers.clear();
      }

      answers.add(answerNumber);
    }

    state.answers[quizNumber] = answers;

    emit(state.copyWith(answers: state.answers));
  }

  void submit() {
    bool allCorrect = true;
    List<Quiz> correctQuizzes = [];
    List<Quiz> inCorrectQuizzes = [];

    for (var quiz in state.quizzes) {
      final answers = state.answers[quiz.number] ?? [];

      if (answers.isEmpty) {
        allCorrect = false;
        inCorrectQuizzes.add(quiz);
        continue;
      }

      if (quiz.isMultiChoice) {
        final correctAnswers = quiz.answers.where((a) => a.isCorrect);

        if (answers.length != correctAnswers.length) {
          allCorrect = false;
          inCorrectQuizzes.add(quiz);
          continue;
        }

        final correct = correctAnswers.every((a) => answers.contains(a.number));
        if (correct) {
          correctQuizzes.add(quiz);
        } else {
          allCorrect = false;
          inCorrectQuizzes.add(quiz);
        }
      } else {
        final correctAnswer = quiz.answers.firstWhere((a) => a.isCorrect);

        if (answers[0] == correctAnswer.number) {
          correctQuizzes.add(quiz);
        } else {
          allCorrect = false;
          inCorrectQuizzes.add(quiz);
        }
      }
    }

    emit(state.copyWith(
      result: () => Result(
        allCorrect: allCorrect,
        correctQuizzes: correctQuizzes,
        inCorrectQuizzes: inCorrectQuizzes,
      ),
    ));
  }

  void reset() {
    emit(state.copyWith(
      answers: {},
      result: () => null,
    ));
  }
}
