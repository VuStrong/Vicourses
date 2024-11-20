
# Vicourses Storage Service

The Storage Service is responsible for uploading and storing files. This service use S3 object storage to store files.

 After a file is uploaded, a JWT token is returned to the user.
  - The token containing the information of the uploaded file and ID of user who uploaded.
  - This token is used by other services, like Course Service to validate the file is uploaded by user or not.

## Technologies
- Node.js
- TypeScript
- Express.js
- RabbitMQ

## Run this service

1. Install the dependencies
   ```shell
   npm install
   ```

2. Set up the environment variables. Rename the `.env.example` file to `.env` and provide the necessary values.

3. Make sure the `public.key` file from [auth_service](https://github.com/VuStrong/Vicourses/tree/main/backend/services/auth_service#run-this-service) is copied to this directory

4. Run
   ```shell
   npm run dev
   ```
Launch the http://localhost:3001/swagger in your browser to view Swagger UI.
