class AppConstants {
  static const String appName = "Vicourses";

  static const int accessTokenLifeTime = 25; // 25 minutes

  static const String defaultCourseImagePath = 'assets/images/course-placeholder.jpg';
  static const String defaultUserImagePath = 'assets/images/default-avatar.png';
}

class Role {
  static const String student = "student";
  static const String instructor = "instructor";
  static const String admin = "admin";
}

class CourseLevel {
  static const String all = "All";
  static const String basic = "Basic";
  static const String intermediate = "Intermediate";
  static const String expert = "Expert";
}

class CourseStatus {
  static const String unpublished = "Unpublished";
  static const String waitingToVerify = "WaitingToVerify";
  static const String published = "Published";
}

class LessonType {
  static const String video = "Video";
  static const String quiz = "Quiz";
}