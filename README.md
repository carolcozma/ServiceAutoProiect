# Auto Service Management System

## Description
This project implements a database management system for an auto service, allowing users to manage cars, services, and spare parts efficiently.

## Features
### User Functionality
- **Login & Registration**
- Two user roles: **Admin** and **User**
- Users can:
  - Add, modify, and request services for their cars
  - Access a catalog of auto parts with images

### Admin Functionality
- View complete service history and modify or delete records
- Manage available auto parts
- Track service history for each mechanic
- Generate statistics:
  - Total number of services
  - Total revenue
  - Low-stock auto parts

## Database Structure
### Tables & Relationships
- **Customers**: Stores client information
- **Cars**: Links cars to customers
- **Services**: Manages performed services
- **Mechanics**: Stores mechanic details
- **CarParts**: Lists available auto parts
- **ServiceParts**: Associates parts with a specific service
- **CarPartTypes**: Defines auto part types
- **ServiceTypes**: Defines available service types

## Integrity Constraints
- **Primary Keys (PK):** Unique identifiers for each table
- **Foreign Keys (FK):** Ensures referential integrity between tables
- **Unique Constraints:** VIN (Cars) and Email (Customers) must be unique
- **NOT NULL Constraints:** Essential fields like FirstName, Password, and Make are required

## CRUD Operations
### INSERT
- **Add Customer**
- **Add Car**
- **Add Service**

### UPDATE
- **Modify Car Information**
- **Modify Service Details**

### DELETE
- **Delete Cars and Associated Services**
- **Delete Services and Related Parts**

## Queries
### Simple Queries
- Get service type statistics
- Retrieve services by car ID
- List parts used in a service
- Rank mechanics by revenue
- Monthly revenue and service count report

### Complex Queries
- Fetch all services with customer & car details
- Identify top 5 customers by service count
- Evaluate mechanic performance
- Retrieve services handled by a specific 
