# Vicourses Client Web

This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app).

## Setup .env file
Before running the project, rename `.env.example` file to `.env`. You may want to change some environment variables below:
- `AUTH_GOOGLE_ID` and `AUTH_GOOGLE_SECRET`: set your Google client ID and secret to use the Login with Google feature.
- `NEXT_PUBLIC_PAYPAL_CLIENT_ID`: set your Paypal client ID to use some features related to Paypal.
- `AUTH_SECRET`: you can change to some longer random secret string.


## Running with Docker
```bash
docker-compose up -d
```

Open [http://localhost:4000](http://localhost:4000) with your browser to see the result.

## Run the local development server:

### 1. Install dependencies

```bash
npm install
```

### 2. Start the app

```bash
npm run dev
```

Open [http://localhost:4000](http://localhost:4000) with your browser to see the result.