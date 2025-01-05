import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class LocalStorage {
  late final FlutterSecureStorage _storage;

  LocalStorage() {
    _storage = const FlutterSecureStorage();
  }

  Future<DateTime?> getDateTime(String key) async {
    String? dateTimeStr = await _storage.read(key: key);

    return dateTimeStr != null ? DateTime.parse(dateTimeStr) : null;
  }

  Future<String?> getString(String key) async {
    return await _storage.read(key: key);
  }

  Future<void> setDateTime({required String key, DateTime? value}) async {
    await _storage.write(key: key, value: value!.toIso8601String());
  }

  Future<void> setString({required String key, String? value}) async {
    await _storage.write(key: key, value: value);
  }

  Future<void> delete(String key) async {
    await _storage.delete(key: key);
  }

  Future<void> deleteAll() async {
    await _storage.deleteAll();
  }
}