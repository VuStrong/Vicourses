class Quiz {
  final int number;
  final String title;
  final bool isMultiChoice;
  final List<Answer> answers;

  Quiz({
    required this.number,
    required this.title,
    required this.isMultiChoice,
    required this.answers,
  });

  static Quiz fromMap(Map<String, dynamic> data) {
    return Quiz(
      number: data['number'],
      title: data['title'],
      isMultiChoice: data['isMultiChoice'],
      answers: (data['answers'] as List)
          .map((e) => Answer(
                number: e['number'],
                title: e['title'],
                isCorrect: e['isCorrect'],
                explanation: e['explanation'],
              ))
          .toList(),
    );
  }
}

class Answer {
  final int number;
  final String title;
  final bool isCorrect;
  final String? explanation;

  Answer({
    required this.number,
    required this.title,
    required this.isCorrect,
    this.explanation,
  });
}
