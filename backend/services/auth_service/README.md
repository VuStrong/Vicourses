
# Vicourses Auth Service

The Auth Service is responsible for user authentication and provides some features like registration, password management, token management, email confirmation.


## Features

- Login with email password or google account.
- Register user
- Refresh token
- Email confirmation
- Reset, change password


## Technologies
 - Node.js
 - TypeScript
 - Express.js
 - MySQL and TypeORM
 - RabbitMQ
## Run this service

1. Install the dependencies
   ```shell
   npm install
   ```

2. Set up the environment variables. Rename the `.env.example` file to `.env` and provide the necessary values for your environment.

3. Build the project and run migrations to create database
   ```shell
   npm run build
   npx typeorm migration:run -d dist/data/data-source.js
   ```
4. Seed data
   ```shell
   npm run seed
   ```
5. Generate RSA Key Pairs (make sure OpenSSL is installed on your machine). The commands below will create 2 files: `private.key` for sign jwt token, keep this file only in this service. `public.key` for verify token, this file can be copied to another services that need to authenticate user.
   ```shell
   openssl genrsa -out private.key -traditional 2048
   openssl rsa -pubout -in private.key -out public.key
   ```
6. Run
   ```shell
   npm run dev
   ```
Go to http://localhost:3000/swagger to view swagger document, the admin account email and password is `admin1@gmail.com` `111111`
