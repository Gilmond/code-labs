# Docker Compose

![Docker Compose Logo](../images/docker-compose.png)

This lab demonstrates usage of [*Docker Compose*](https://docs.docker.com/compose/).

## Overview

In this lab we'll extend upon the [ASP.Net MVC](./asp-net-mvc.md) and [Docker](./docker-basics.md) labs completed previously. We'll introduce [scaling](https://en.wikipedia.org/wiki/Scalability) through *Docker Compose* and tackle the problems that arise as a result of this.

> You **must** have completed the [previous two labs](https://github.com/gilmond/code-labs) to complete this lab.

### Notes

* *IDE*: As with the *ASP.Net MVC* and *Docker* labs, whilst this lab *can* be completed with Visual Studio (or any IDE of your preference), we recommend and have written this guide for *VS Code*.

## Introduction

*Compose* is a tool for defining and running multi-container applications. *Compose* uses a project name to isolate environments from one another, allowing for multiple isolated environments on a single host. Any declared *volumes* are preserved and transferred from previous containers to new containers to ensure any data isn't lost. *Compose* also tracks changes to your container configurations so that deployments can re-use existing containers that do not need to be re-deployed.

These features and the simplicity of it's tooling makes this an invaluable tool for development, testing, and staging. However, services your applications depend on such as *MS SQL Server*, *Redis Cache*, *RabbitMQ*, etc. are unlikely to be deployed to production in the same manner. High availability, geo-redundant services are possible, but these dependent services will often be dedicated cloud resources or deployed to powerful servers outside of docker's remit.

*Compose* is completely compatible with [*Docker Swarm*](https://docs.docker.com/engine/swarm/), which itself and/or combined with *orchestration* tooling such as [Kubernetes](https://kubernetes.io/), [Docker Enterprise](https://www.docker.com/enterprise-edition), [Azure Container Service](https://azure.microsoft.com/en-us/blog/azure-container-service-preview/), etc. can securely link your *application* containers to these dependent services, making for a powerful and complete lifecycle.

## Part 1: Compose File

The first step in using *Compose* is to define a `Dockerfile`, which we completed in the previous lab:

```docker
FROM microsoft/aspnetcore:1.1.2
WORKDIR /app
COPY bin/Debug/netcoreapp1.1/publish/ .
ENTRYPOINT ["dotnet", "TodoApplication.dll"]
EXPOSE 80
```

The second step is to define a `docker-compose.yml` file at the root of your solution. The first section we add determines the [version](https://docs.docker.com/compose/compose-file/compose-versioning/) of *Compose* we want to work with. The latest stable recommended version is `3`:

> _**Note**: Unlike with your plain text `Dockerfile`, your `docker-compose.yml` file uses the [`yaml`](https://en.wikipedia.org/wiki/YAML) format, which is a human-readable serialization language commonly used in configuration files. If you have not worked with `yaml` before, it is important to note that **spaces matter**. That is, when you see indentiation in a `yaml` file, **use spaces, not tabs**._

```yml
version: '3'
```

Now we'll declare the services we want to use. For the time being, the only service we care about is the API we created previously. There are [several elements](https://docs.docker.com/compose/compose-file/#service-configuration-reference) we can configure for our services, but for now we'll concentrate on these points:

* Name - the name docker will use to refer to our service
* Build - the relative path to the directory containing our `Dockerfile` for the service
* Ports - the ports we want to bind

```yml
api:
  build: ./src/TodoApplication
  ports: 6000:80
```

As is hopefully evident, the above configuration defines our service with the *name* "api", points to the `Dockerfile` relative from your `docker-compose.yml`, and binds port 6000 on your host to port 80 on the container (which was the port defined in your `Dockerfile` under `EXPOSE`).

> _**Note**: Indentation shown uses two normal spaces. Be careful, especially when using 4 spaces, that your IDE doesn't replace the spaces with tabs._

The complete `docker-compose.yml` file should look as follows:

```yml
version: '3'
services:
  api:
    build: ./src/TodoApplication
    ports: 6000:80
```

## Part 2: Compose Commands

The third step is to deploy our service:

* Press `Ctrl + Shift + '` *(`Terminal: Create new integrated Terminal`)* to bring up the terminal.
  * Ensure the terminal has opened the in the solution (root) directory. If it hasn't, navigate to it using `cd`.
* Use the command `docker compose up`.