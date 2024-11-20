
# Vicourses Email Service

The Email Service is responsible for sending email to users. 

## Technologies
 - Node.js
 - TypeScript
 - RabbitMQ
 - EJS Template Engine


## Run this service

1. Install the dependencies
   ```shell
   npm install
   ```

2. Set up the environment variables. Rename the `.env.example` file to `.env` and provide the necessary values for SMTP and RabbitMQ key.

3. Run
   ```shell
   npm run dev
   ```
This service will be running on port 3002. It consume messages from RabbitMQ **send_email** queue and send email to users.
    
## Input to send email

Messages come from RabbitMQ must be JSON string with the following structure: 

```javascript
{
  "to": "abc@gmail.com",
  "template": "template-name",
  "payload": {
    "some-key": "some-value",
    ...
  }
}
```


## Available templates
Available templates located in the `templates` directory of the repo.
- `confirm_email`: The email sent to users when they create an account.
- `reset_password`: The email sent to users when they request a password reset.
- `course_approved`: The email sent to users when one of their courses is approved.
- `course_not_approved`: The email sent to users when one of their courses is not approved.