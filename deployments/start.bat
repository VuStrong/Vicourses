openssl genrsa -out ./services/user_service/private.key -traditional 2048
openssl rsa -pubout -in ./services/user_service/private.key -out ./services/user_service/public.key

docker-compose -p vicourses up -d