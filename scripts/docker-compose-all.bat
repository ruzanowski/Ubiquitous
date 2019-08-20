docker-compose -f ../elk/docker-compose.yml up
TIMEOUT /T 2

docker-compose -f ../docker-compose-infrastructure.yml up
TIMEOUT /T 2

docker-compose -f ../docker-compose-services.yml up
TIMEOUT /T 5