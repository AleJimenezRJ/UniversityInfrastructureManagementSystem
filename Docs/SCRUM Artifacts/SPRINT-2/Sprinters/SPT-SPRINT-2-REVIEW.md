# Sprint Review Report – Sprint 2

**Project:** ThemePark\@UCR

**Sprint Dates:** 05/28/2025 - 06/18/2025

**Review Date:** 06/18/2025

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

Implement user authentication and authorization functionalities, including user and role management, to ensure secure access control.
Added as a user log management system to track user activities and changes. Standardize the interface design across all views to ensure a consistent user experience.

---

## 2. Accepted Product Backlog Items

* **SPT-AU-002-005 Log out of the system**
* **SPT-AU-002-003 Integrate backend with Microsoft B2C logout flow**
* **SPT-AU-002-001 Validate Microsoft Auth B2C tokens in backend**
* **SPT-UI-006-003 Paginated user listings**
* **SPT-UM-005-005 List User Audit Logs**
* **SPT-SR-007-001 Unified and reliable error handling**
* **SPT-UI-006-009 User audit logs window**
* **SPT-UI-006-008 Standardize UI components in user management**
* **SPT-UI-006-005 Edit user via page window**
* **SPT-AU-002-002 Display error on login failure**

---

## 3. Rejected Product Backlog Items

* **SPT-AU-002-006 Enforce role based access control:** Authorization was not implemented in the backend, so this PBI was rejected.
* **SPT-AU-002-004 Log in to the system:** Rejected due to a bug in the authorization system that prevented successful login.

---

## 4. Feedback Received

* The backend must be able to handle user roles and permissions correctly.
* The authentication system needs to be more robust, as there were issues with login and token validation.

---

## 5. Actions and Next Steps

* Refactor the authentication and authorization system to resolve existing bugs.
* Implement role-based access control in the backend to ensure proper authorization.
* Start tasks earlier to avoid last-minute rushes and ensure timely delivery of PRs.
* Allow any team member to merge changes, provided they ensure the code compiles and the database is functioning correctly.
