// int
extension FormatLengthExtension on int {
  String toDurationString() {
    final second = this % 60;
    final minute = (this / 60).floor() % 60;
    final hour = (this / 60 / 60).floor();

    final hourStr = hour < 10 ? '0$hour' : '$hour';
    final minuteStr = minute < 10 ? '0$minute' : '$minute';
    final secondStr = second < 10 ? '0$second' : '$second';

    return hour == 0
        ? '$minuteStr:$secondStr'
        : '$hourStr:$minuteStr:$secondStr';
  }
}
