# Web Project

## Overview
The Web project is the presentation layer of the application. It exposes APIs and handles Grpc requests/responses.

## Contents
- **Controllers**: Handle incoming Grpc requests.
- **Middleware**: Add cross-cutting functionality to the request pipeline.

## Key Concepts
- **Grpc APIs**: Expose Grpc Services for interacting with the application.
- **Dependency Injection**: Uses DI to inject services from other layers.
