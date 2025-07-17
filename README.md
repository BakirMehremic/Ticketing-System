# Ticketing System API

RESTful API project for sending tickets, responses, attachments, and everything a ticketing system should have.  
It uses **.NET 8**, **PostgreSQL**, **Redis**, **JWTs**, etc.  
The system supports regular user roles, admin roles, and department-specific accesses.

---

##  Architecture notes

- Follows a standard **Controller-Service-Repository** pattern with middleware to handle unexpected and custom-thrown exceptions.

- **Generic Repository Pattern** and **Query Builder Pattern** are used to minimize code duplication.
- **Redis** is used to cache user roles separately from JWTs, supporting both custom login and the previously used OAuth (where roles were not included in tokens).

---

## Run Locally

Docker Compose has the needed variables set to run locally.

**Run the following commands:**

```bash
git clone git@github.com:BakirMehremic/Ticketing-System.git
cd Ticketing-System
docker compose up --build
