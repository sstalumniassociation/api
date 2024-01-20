# SST Alumni Association API

The next generation of the SSTAA App API, featuring support for gRPC and REST APIs.

## Getting started

Please ensure you have installed the software listed below:

* .NET SDK v8
* Docker with Docker Compose

To get started:

> Docker commands may require `sudo`, depending on your machines configuration.

1. Start the database

```shell
docker compose up -d
```

This will create:

* `postgres` accessible on port 5432
* `adminer` accessible on port 8080

The `postgres` instance can be accessed using:

* Username: `postgres`
* Password: `sa`
* Database: `postgres`

2. Liftoff!

```shell
dotnet run --project Api
```

Launch the Swagger interface at <http://localhost:5200>

## Sibling projects

[TypeScript API Client](https://github.com/sstalumniassociation/api-client-typescript)

[Web App](https://github.com/sstalumniassociation/web)

[iOS App](https://github.com/sstalumniassociation/ios)
