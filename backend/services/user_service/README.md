
# Vicourses User Service

The User Service is responsible for managing and authenticating users.


## Features

- Login with email password or google account.
- User registration.
- Refresh token.
- Email confirmation, password reset.
- User can manage their profile information.
- Admin can manage other users (view, lock account).


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

3. Create database in your MySQL server (database name is the value of `DB_DATABASE` in `.env` file) 

4. Run migrations to create tables
   ```shell
   npm run migration:run
   ```
5. Seed data
   ```shell
   npm run seed
   ```
6. Generate RSA Key Pairs (make sure OpenSSL is installed on your machine). The commands below will create 2 files: `private.key` for sign jwt token, keep this file only in this service. `public.key` for verify token, this file can be copied to another services that need to authenticate user.
   ```shell
   openssl genrsa -out private.key -traditional 2048
   openssl rsa -pubout -in private.key -out public.key
   ```
7. Run
   ```shell
   npm run dev
   ```
Go to http://localhost:3000/swagger to view swagger document, the admin account email and password is `admin1@gmail.com` `11111111`

# Communicate with other microservices

- Send messages to `send_email` queue (email confirmation link, password reset link). So you need to keep [email_service](https://github.com/VuStrong/Vicourses/tree/main/backend/services/email_service) running to consume that messages and send.
- After create an user, publish a message including the user information to RabbitMQ `user.created` exchange.
- After update an user, publish a message including the user information to RabbitMQ `user.info.updated` exchange.
