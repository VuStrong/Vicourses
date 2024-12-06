host="localhost:27017"
username="root"
password="123456"

mongoimport --uri "mongodb://$username:$password@$host/course_db" --collection categories --authenticationDatabase admin --file "/tmp/course_db.categories.json" --jsonArray

mongoimport --uri "mongodb://$username:$password@$host/course_db" --collection courses --authenticationDatabase admin --file "/tmp/course_db.courses.json" --jsonArray

mongoimport --uri "mongodb://$username:$password@$host/course_db" --collection lessons --authenticationDatabase admin --file "/tmp/course_db.lessons.json" --jsonArray

mongoimport --uri "mongodb://$username:$password@$host/course_db" --collection sections --authenticationDatabase admin --file "/tmp/course_db.sections.json" --jsonArray

mongoimport --uri "mongodb://$username:$password@$host/course_db" --collection users --authenticationDatabase admin --file "/tmp/course_db.users.json" --jsonArray

mongoimport --uri "mongodb://$username:$password@$host/wishlist_db" --collection courses --authenticationDatabase admin --file "/tmp/wishlist_db.courses.json" --jsonArray