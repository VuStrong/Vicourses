class PagedResult<T> {
  PagedResult({
    required this.items,
    this.total = 0,
    this.skip = 0,
    this.limit = 0,
    this.end = false,
  });

  final List<T> items;
  final int total;
  final int skip;
  final int limit;
  final bool end;
}
