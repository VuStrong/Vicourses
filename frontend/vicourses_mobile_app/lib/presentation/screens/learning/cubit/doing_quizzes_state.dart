part of 'doing_quizzes_cubit.dart';

class DoingQuizzesState {
  final List<Quiz> quizzes;
  final Map<int, List<int>> answers;
  final Result? result;

  DoingQuizzesState({
    required this.quizzes,
    required this.answers,
    this.result,
  });

  bool get canSubmit {
    for (var quiz in quizzes) {
      if (answers[quiz.number] == null || answers[quiz.number]!.isEmpty) {
        return false;
      }
    }

    return true;
  }

  DoingQuizzesState copyWith({
    List<Quiz>? quizzes,
    Map<int, List<int>>? answers,
    Result? Function()? result,
  }) {
    return DoingQuizzesState(
      quizzes: quizzes ?? this.quizzes,
      answers: answers ?? this.answers,
      result: result != null ? result() : this.result,
    );
  }
}

class Result {
  final bool allCorrect;
  final List<Quiz> correctQuizzes;
  final List<Quiz> inCorrectQuizzes;

  Result({
    this.allCorrect = false,
    required this.correctQuizzes,
    required this.inCorrectQuizzes,
  });
}
