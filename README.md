# Getting hands dirty with ASP.NET
It's not even a complete production ready project, but my attemt to dive in C# high-level ecosystem.

API consists of three controllers, which will also be visible on Swagger (with integrated authentication token):
 - one for registering and authenticating users
 - one for CRD operations on Job entities
 - one for RUD operations on User entities

# Running with Docker
you will need docker compose plugin to build image from ```docker-compose.yaml``` file:
```
docker compose build
```
and start container:
```
docker compose up
```
API service in ```docker-compose.yaml``` file is configured so that it waits for SQL Server container to get fully initialized.
Therefore, it will take some time (less than a minute) to get everything up and running.

To stop containers you can:
 - either press ```CTRL+C``` and then manually cleanup unused space using ```docker conatiner prune``` and
   remove created default network bridge using ```docker network rm update_default```.
 - or open another terminal session and run ```docker compose down``` command.
