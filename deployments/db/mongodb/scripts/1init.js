db = db.getSiblingDB('admin');
db.auth("root", "123456");
db.createUser({
  user: "courseservice",
  pwd: "123456",
  roles: [{ role: "readWrite", db: "course_db" }],
});
db.createUser({
  user: "wishlistservice",
  pwd: "123456",
  roles: [{ role: "readWrite", db: "wishlist_db" }],
});
