# customer-account
## Installation
Run the below command
```sh
docker-compose up -d
```
*Alternatively VS can be used to run the solution.*
## Cleanup
Run the below commands
```sh
docker-compose down -v --rmi all
```
*Alternatively VS can be used to clean resources*
## Introduction
The project consists of two microservices that communicate with each other using Events.
### Technologies
- `MongoDb` used for persistence
- `RabbitMQ` used as message broker
- `NET 5` used for the Microservices
- `NETstandard 2.1` used for packages
- `Swagger` used as Documentation UI
- `Docker` used for Containerisation
- `Compose` used for dependencies
- `GitHub Actions` used for CI
- `DockerHub` used for Image persistence
 
 
## Building Blocks
 
### Customer API
- URI: http://localhost:81/swagger/index.html
- Description: Responsible to Manage Customers
 
Swagger Can be used in order to Create a Customer.
 
Once a customer is created with a positive Event will be sent to Account microservice.
 
 
### Account API
- URI: http://localhost:82/swagger/index.html
- Description: Responsible to Manage Account Transactions and Balance
 
Swagger Can be used in order to Extract Account Information and Balance. Additional operations can be done on the Account (Deposit and Withdrawal).
 
 
 
 

