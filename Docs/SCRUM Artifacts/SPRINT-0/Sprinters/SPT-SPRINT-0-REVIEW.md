# **Sprint Review Report**

## **1. General Information**

* **Sprint Number:** Sprint 0
* **Team Name:** ThemePark\@UCR – Sprinters
* **Meeting Date:** May 7th, 2025

## **2. User Stories Reviewed and Validated by the PO**

| Story ID       | User Story                                                             | Team Members                                      | Story Points |
| -------------- | ---------------------------------------------------------------------- | ------------------------------------------------- | ------------ |
| SPT-UM-001-001 | As a system administrator, I want to view a list of all registered people.        | Elizabeth Huang Wu, Andrés Murillo Murillo        | 3            |
| SPT-UM-001-002 | As a user, I want to read a person’s info in the system.                      | Andrés Murillo Murillo, Gael Alpizar Alfaro       | 3            |
| SPT-UM-001-003 | As a user, I want to create a new person in the system.                | Esteban Baires Cerdas, Gypsi Tatiana Páramo López | 8            |
| SPT-UM-001-004 | As an admin, I want to update a person's information in the system .                  | Isabella Rodríguez Sánchez, Gael Alpizar Alfaro   | 5            |
| SPT-UM-001-005 | As a person, I want to delete all data related to a person. | Elizabeth Huang Wu, Isabella Rodríguez Sánchez    | 3            |

## **3. Accepted and Rejected User Stories**

### **3.1 Accepted Stories**

| Story ID       | User Story                        | Points |
| -------------- | --------------------------------- | ------ |
| SPT-UM-001-001 | View list of registered people    | 3      |
| SPT-UM-001-002 | Read a person’s information       | 3      |
| SPT-UM-001-003 | Create a new person in the system | 8      |
| SPT-UM-001-004 | Update a person’s information     | 5      |
| SPT-UM-001-005 | Delete a person’s data            | 3      |

**Total accepted points:** 22

**Accepted points per team member:**

* Elizabeth Huang Wu: 6
* Andrés Murillo Murillo: 6
* Gael Alpizar Alfaro: 8
* Esteban Baires Cerdas: 8
* Gypsi Tatiana Páramo López: 8
* Isabella Rodríguez Sánchez: 8

### **3.2 Rejected Stories**

* None.

## **4. PO and Stakeholders Feedback**

During the Sprint Review, the Product Owner and stakeholders gave the following feedback:

**Personal data:**

* Add support for second names and second last names.
* Allow registering multiple email addresses per person, including an alternative one.

**Internationalization and validations:**

* All feedback must be in Spanish or the defined language.
* Translations should be validated with the PO to ensure consistency.
* Explore the possibility of adapting the system for other universities.

**Security and endpoint organization:**

* Properly protect links within the API endpoints.
* Review whether external users should be allowed to access the Swagger documentation.

**System architecture:**

* Group services and endpoints by functionality — currently, things are a bit disorganized.

**Data and account management:**

* Keep records of people even after their accounts are deleted — this could be useful for future analysis or sales.
* In future sprints, prioritize improving the "Modify Person" service: remove non-editable fields and enhance the interface.
* Confirm with the PO whether import/export features (like Excel files) are needed.

## **5. Sprint Velocity**

* Story points committed at the beginning of the sprint: 22
* Story points completed (accepted): 22

## **6. Artifacts Generated During the Meeting**

* A single computer was used to present the implemented features.
* The team showcased their GitHub backlog live during the meeting.

**Additional notes:**

* For next sprint reviews, it was suggested to print PBIs so the PO can validate them more interactively.
* It was also proposed to define better timeboxes for each section of the demo.