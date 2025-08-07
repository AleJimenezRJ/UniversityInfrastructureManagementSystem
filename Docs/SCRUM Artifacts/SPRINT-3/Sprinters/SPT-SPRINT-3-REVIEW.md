# Sprint Review Report – Sprint 3

**Project:** ThemePark\@UCR

**Sprint Dates:** 06/19/2025 - 07/09/2025
**Review Date:** 07/09/2025
**Development Team:** Sprinters

---

**Team Members and Roles:**

* Andrés Murillo Murillo – Scrum Master
* Esteban Isaac Baires Cerdas – Scrum Ambassador
* Gael Alpízar Alfaro
* Elizabeth Huang Wu
* Isabella Rodríguez Sánchez
* Gypsi Tatiana Páramo López

---

## 1. Sprint Goal

Refactor the authentication and authorization system to improve efficiency and security. This included enriching authentication tokens for faster access control validation and implementing proper role and permission management in the backend.

---

## 2. Accepted Product Backlog Items

* **SPT-AI-001-001 Retrieve Recent Modifications for User via Identity Number** (#386)
* **SPT-AU-002-004 Log in to the system** (#257)
* **SPT-UI-006-010 Instant Search in User Management Table** (#395)
* **SPT-CPD-SQL-PQL-DP-001-001 Deploy application** (#442)
* **SPT-AU-002-007 Register a new user in the system** (#400)
* **SPT-AU-002-006 Enforce role based access control** (#259)

---

## 3. Rejected Product Backlog Items

None. All PBIs planned for this sprint were accepted.

---

## 4. Feedback Received

* The search feature in the User Management window does not include the Phone Number column. This was noted but considered acceptable, as searching across the entire database can become a heavy query and might not always be advisable.

---

## 5. Actions and Next Steps

* Evaluate the feasibility of optimizing database queries to allow optional search by Phone Number without significant performance impact.
* Continue reinforcing backend authorization checks for edge cases.
* Acquire and configure an SSL/TLS certificate to enable secure publishing of the application.
* Ensure the database is not directly exposed to the internet, following best practices for security and architecture.
