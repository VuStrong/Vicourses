FROM node:lts-alpine AS builder

WORKDIR /app

COPY ./package.json ./
RUN npm install

COPY . .

RUN npm run build

FROM nginx

COPY --from=builder /app/dist /usr/share/nginx/html

COPY ./docker/nginx.conf /etc/nginx/conf.d/default.conf