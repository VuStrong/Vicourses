class Category {
  final String id;
  final String name;
  final String slug;
  final String? parentId;

  Category({
    required this.id,
    required this.name,
    required this.slug,
    this.parentId,
  });

  static Category fromMap(Map<String, dynamic> data) {
    return Category(
      id: data['id'],
      name: data['name'],
      slug: data['slug'],
      parentId: data['parentId'],
    );
  }
}
