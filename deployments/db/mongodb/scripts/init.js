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

// Seed course_db
db = db.getSiblingDB('course_db');
db.courses.insertMany([{
  "_id": "V90Dh8ykUsci",
  "Title": "React - The Complete Guide 2024 (incl. Next.js, Redux)",
  "TitleCleaned": "react-the-complete-guide-2024-incl-nextjs-redux",
  "Description": "This course will teach you React.js in a practice-oriented way, using all the latest patterns and best practices you need. You will learn all the key fundamentals as well as advanced concepts and related topics to turn you into a React.js developer.",
  "Tags": [
    "reactjs",
    "frontend"
  ],
  "Requirements": [],
  "TargetStudents": [],
  "LearnedContents": [],
  "Level": "Basic",
  "IsPaid": true,
  "Price": {
    "$numberDecimal": "24.99"
  },
  "Rating": {
    "$numberDecimal": "4.0000000000000000000000000000"
  },
  "CreatedAt": {
    "$date": "2024-09-19T16:29:48.686Z"
  },
  "UpdatedAt": {
    "$date": "2024-11-09T07:49:50.115Z"
  },
  "StudentCount": 2,
  "Locale": {
    "Name": "vi",
    "EnglishTitle": "Vietnamese"
  },
  "IsApproved": true,
  "Status": "Published",
  "Thumbnail": {
    "FileId": "test2.png",
    "Url": "https://r2.strongtify.io.vn/test2.png"
  },
  "PreviewVideo": {
    "FileId": "30MinuteVideo720p.mp4",
    "Url": "https://r2.strongtify.io.vn/30MinuteVideo720p.mp4",
    "OriginalFileName": "",
    "StreamFileUrl": "https://r2.strongtify.io.vn/hls/c93e887b-6a20-4949-840c-e81a9ad84b95/master.m3u8",
    "Duration": 1809,
    "Status": "Processed"
  },
  "Category": {
    "_id": "421679",
    "Name": "Development",
    "Slug": "development"
  },
  "SubCategory": {
    "_id": "698569",
    "Name": "Web Development",
    "Slug": "web-development"
  },
  "User": {
    "_id": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
    "Name": "teacher 1",
    "ThumbnailUrl": null
  }
},
{
  "_id": "FtDOrVhpmBQ6",
  "Title": "NestJS Masterclass - NodeJS Framework Backend Development",
  "TitleCleaned": "nestjs-masterclass-nodejs-framework-backend-development",
  "Description": "NestJS Masterclass is a Practical Course!  We work together to build a REST API server-side application for a blog. We learn while we code this application, so all the examples in this course are real-world use cases. While programming this application, we will learn various NestJS features and dive deeper into the internal mechanics of NestJS.",
  "Tags": [],
  "Requirements": [],
  "TargetStudents": [],
  "LearnedContents": [],
  "Level": "All",
  "IsPaid": true,
  "Price": {
    "$numberDecimal": "4.99"
  },
  "Rating": {
    "$numberDecimal": "0"
  },
  "CreatedAt": {
    "$date": "2024-09-19T16:31:07.068Z"
  },
  "UpdatedAt": {
    "$date": "2024-11-06T11:33:57.013Z"
  },
  "StudentCount": 0,
  "Locale": null,
  "IsApproved": true,
  "Status": "Published",
  "Thumbnail": null,
  "PreviewVideo": null,
  "Category": {
    "_id": "421679",
    "Name": "Development",
    "Slug": "development"
  },
  "SubCategory": {
    "_id": "698569",
    "Name": "Web Development",
    "Slug": "web-development"
  },
  "User": {
    "_id": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
    "Name": "teacher 1",
    "ThumbnailUrl": null
  }
},
{
  "_id": "SgOyEZIhNu55",
  "Title": "Khóa học Figma từ căn bản đến thực chiến",
  "TitleCleaned": "khoa-hoc-figma-tu-can-ban-en-thuc-chien",
  "Description": "Khóa học thiết kế giao diện bằng Figma dành cho những bạn có đam mê với ngành nghề UI/UX design. Khóa học tập trung vào những kỹ năng căn bản nhất, đồng thời cung cấp một cái nhìn tổng quát giúp học viên có thể tạo ra sản phẩm cụ thể sau khóa học.",
  "Tags": [],
  "Requirements": [],
  "TargetStudents": [],
  "LearnedContents": [],
  "Level": "All",
  "IsPaid": false,
  "Price": {
    "$numberDecimal": "0"
  },
  "Rating": {
    "$numberDecimal": "0"
  },
  "CreatedAt": {
    "$date": "2024-09-19T16:33:14.417Z"
  },
  "UpdatedAt": {
    "$date": "2024-10-04T15:02:09.597Z"
  },
  "StudentCount": 0,
  "Locale": null,
  "IsApproved": true,
  "Status": "Published",
  "Thumbnail": null,
  "PreviewVideo": null,
  "Category": {
    "_id": "688645",
    "Name": "Design",
    "Slug": "design"
  },
  "SubCategory": {
    "_id": "593034",
    "Name": "Degisn Tools",
    "Slug": "degisn-tools"
  },
  "User": {
    "_id": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
    "Name": "teacher 1",
    "ThumbnailUrl": null
  }
}]);
db.users.insertMany([
{
  "_id": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
  "Name": "teacher 1",
  "ThumbnailUrl": null,
  "Email": "teacher1@gmail.com",
  "EnrolledCoursesVisible": true
},
{
  "_id": "00543305-2d30-4520-9f5b-f35a58931338",
  "Name": "admin 1",
  "ThumbnailUrl": null,
  "Email": "admin1@gmail.com",
  "EnrolledCoursesVisible": true
}]);
db.categories.insertMany([{
  "_id": "421679",
  "Name": "Development",
  "Slug": "development",
  "ParentId": null,
  "CreatedAt": {
    "$date": "2024-09-15T13:33:35.381Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-15T13:33:35.381Z"
  }
},
{
  "_id": "688645",
  "Name": "Design",
  "Slug": "design",
  "ParentId": null,
  "CreatedAt": {
    "$date": "2024-09-15T13:36:25.262Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-15T13:36:25.262Z"
  }
},
{
  "_id": "698569",
  "Name": "Web Development",
  "Slug": "web-development",
  "ParentId": "421679",
  "CreatedAt": {
    "$date": "2024-09-15T13:37:06.021Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-15T13:37:06.021Z"
  }
},
{
  "_id": "824734",
  "Name": "Mobile Apps Development",
  "Slug": "mobile-apps-development",
  "ParentId": "421679",
  "CreatedAt": {
    "$date": "2024-09-15T13:38:21.218Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-16T10:56:31.179Z"
  }
},
{
  "_id": "532474",
  "Name": "Game Degisn",
  "Slug": "game-degisn",
  "ParentId": "688645",
  "CreatedAt": {
    "$date": "2024-09-15T13:39:32.309Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-15T13:39:32.309Z"
  }
},
{
  "_id": "589649",
  "Name": "Graphic Degisn",
  "Slug": "graphic-degisn",
  "ParentId": "688645",
  "CreatedAt": {
    "$date": "2024-09-15T13:40:06.974Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-15T13:40:06.974Z"
  }
},
{
  "_id": "593034",
  "Name": "Degisn Tools",
  "Slug": "degisn-tools",
  "ParentId": "688645",
  "CreatedAt": {
    "$date": "2024-09-15T13:47:51.292Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-15T13:47:51.292Z"
  }
}]);
db.sections.insertMany([{
  "_id": "Ulx4aMiEaM6AKC",
  "CourseId": "V90Dh8ykUsci",
  "UserId": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
  "Title": "Getting Started",
  "Description": "sec",
  "Order": 1,
  "CreatedAt": {
    "$date": "2024-09-24T13:49:57.512Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-26T11:57:26.486Z"
  }
},
{
  "_id": "1vDW02SQqqofw6",
  "CourseId": "V90Dh8ykUsci",
  "Title": "Javascript Refresher",
  "Description": null,
  "Order": 2,
  "CreatedAt": {
    "$date": "2024-09-24T13:50:39.744Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-24T13:50:39.744Z"
  },
  "UserId": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9"
},
{
  "_id": "5qEp1nMQvJDYwb",
  "CourseId": "V90Dh8ykUsci",
  "Title": "React Essentials - Components, JSX, Props, State & More",
  "Description": null,
  "Order": 3,
  "CreatedAt": {
    "$date": "2024-09-24T13:52:31.690Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-24T13:52:31.690Z"
  },
  "UserId": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9"
},
{
  "_id": "bwhY1c96wNq1Od",
  "CourseId": "FtDOrVhpmBQ6",
  "Title": "Introduction to NestJS",
  "Description": null,
  "Order": 1,
  "CreatedAt": {
    "$date": "2024-09-24T14:14:23.070Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-24T14:14:23.070Z"
  },
  "UserId": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9"
}]);
db.lessons.insertMany([{
  "_id": "54tdkeoWnjLGrN",
  "CourseId": "V90Dh8ykUsci",
  "SectionId": "Ulx4aMiEaM6AKC",
  "UserId": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
  "Title": "Welcome to the course!",
  "Description": null,
  "Order": 1,
  "Type": "Video",
  "Video": null,
  "CreatedAt": {
    "$date": "2024-09-24T14:17:03.690Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-26T13:35:07.642Z"
  }
},
{
  "_id": "Rk1xF3Jge8Ag9s",
  "CourseId": "V90Dh8ykUsci",
  "SectionId": "Ulx4aMiEaM6AKC",
  "Title": "What is React.js? And Why Would You Use It?",
  "Description": null,
  "Order": 2,
  "Type": "Video",
  "Video": null,
  "CreatedAt": {
    "$date": "2024-09-24T14:18:45.482Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-24T14:18:45.482Z"
  },
  "UserId": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9"
},
{
  "_id": "0T4f4FTi5yF9s8",
  "CourseId": "V90Dh8ykUsci",
  "SectionId": "1vDW02SQqqofw6",
  "Title": "Module Introduction",
  "Description": null,
  "Order": 3,
  "Type": "Video",
  "Video": null,
  "CreatedAt": {
    "$date": "2024-09-24T14:21:06.178Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-24T14:21:06.178Z"
  },
  "UserId": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9"
},
{
  "_id": "o2QGB5CHpwd6mb",
  "CourseId": "FtDOrVhpmBQ6",
  "SectionId": "bwhY1c96wNq1Od",
  "Title": "NestJS Masterclass Introduction",
  "Description": null,
  "Order": 1,
  "Type": "Video",
  "Video": null,
  "CreatedAt": {
    "$date": "2024-09-24T14:23:45.456Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-24T14:23:45.456Z"
  },
  "UserId": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9"
},
{
  "_id": "kbkwoDCl1OY6j1",
  "CourseId": "V90Dh8ykUsci",
  "SectionId": "Ulx4aMiEaM6AKC",
  "UserId": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
  "Title": "Some questions about ReactJS",
  "Description": null,
  "Order": 0,
  "Type": "Quiz",
  "CreatedAt": {
    "$date": "2024-09-28T13:08:21.821Z"
  },
  "UpdatedAt": {
    "$date": "2024-09-28T13:08:21.821Z"
  },
  "Video": null
}]);
db.quizzes.insertMany([{
  "_id": "JGOA0UO0kpmwEu",
  "Title": "Which programming language used to code ReactJS ??",
  "Number": 2,
  "IsMultiChoice": false,
  "UserId": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
  "Answers": [
    {
      "Number": 1,
      "Title": "Java",
      "IsCorrect": false,
      "Explanation": null
    },
    {
      "Number": 2,
      "Title": "Javascript",
      "IsCorrect": true,
      "Explanation": "Yeah it Javascript, (or maybe Typescript if you like some types :>>)"
    },
    {
      "Number": 3,
      "Title": "C++",
      "IsCorrect": false,
      "Explanation": null
    }
  ],
  "LessonId": "kbkwoDCl1OY6j1"
},
{
  "_id": "rVST07xlyTB5V9",
  "Title": "Why use ReactJS ??",
  "Number": 1,
  "IsMultiChoice": true,
  "UserId": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
  "Answers": [
    {
      "Number": 1,
      "Title": "React provides state-of-the-art functionality and is an excellent choice for developers looking for an easy-to-use and highly productive JavaScript framework",
      "IsCorrect": true,
      "Explanation": null
    },
    {
      "Number": 2,
      "Title": "To code a backend system",
      "IsCorrect": false,
      "Explanation": null
    },
    {
      "Number": 3,
      "Title": "Using React, you can build complex UI interactions that communicate with the server in record time with JavaScript-driven pages",
      "IsCorrect": true,
      "Explanation": null
    }
  ],
  "LessonId": "kbkwoDCl1OY6j1"
}]);

// Seed wishlist_db
db = db.getSiblingDB('wishlist_db');
db.courses.insertMany([{
  "_id": "V90Dh8ykUsci",
  "Title": "React - The Complete Guide 2024 (incl. Next.js, Redux)",
  "TitleCleaned": "react-the-complete-guide-2024-incl-nextjs-redux",
  "IsPaid": false,
  "Price": {
    "$numberDecimal": "0"
  },
  "Rating": {
    "$numberDecimal": "4.0000000000000000000000000000"
  },
  "ThumbnailUrl": "https://r2.strongtify.io.vn/test2.png",
  "User": {
    "_id": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
    "Name": "teacher 1",
    "ThumbnailUrl": null
  },
  "Status": "Published"
},
{
  "_id": "FtDOrVhpmBQ6",
  "Title": "NestJS Masterclass - NodeJS Framework Backend Development",
  "TitleCleaned": "nestjs-masterclass-nodejs-framework-backend-development",
  "IsPaid": true,
  "Price": {
    "$numberDecimal": "4.99"
  },
  "Rating": {
    "$numberDecimal": "0"
  },
  "ThumbnailUrl": null,
  "User": {
    "_id": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
    "Name": "teacher 1",
    "ThumbnailUrl": null
  },
  "Status": "Published"
},
{
  "_id": "SgOyEZIhNu55",
  "Title": "Khóa học Figma từ căn bản đến thực chiến",
  "TitleCleaned": "khoa-hoc-figma-tu-can-ban-en-thuc-chien",
  "IsPaid": false,
  "Price": {
    "$numberDecimal": "0"
  },
  "Rating": {
    "$numberDecimal": "0"
  },
  "ThumbnailUrl": null,
  "User": {
    "_id": "5a8a8a8c-4663-41b5-9849-81ae7f6726e9",
    "Name": "teacher 1",
    "ThumbnailUrl": null
  },
  "Status": "Published"
}]);