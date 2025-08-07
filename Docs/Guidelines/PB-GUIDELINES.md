# Backlog

---
## Table of Contents

- [Issues](#issues)  
  - [Identification Structure](#issue-identification-structure)  
  - [Epic Details](#epic-details)  
  - [Issue Details](#issue-details)

- [User Story](#user-story)  
  - [User Story Format](#user-story-format)

- [Acceptance Criteria](#acceptance-criteria)  
  - [Acceptance Criteria Format](#acceptance-criteria-format)  
  - [Acceptance Criteria Guidelines](#acceptance-criteria-guidelines)

- [Non-Functional Requirements](#non-functional-requirements)  
  - [Common NFR Categories](#common-nfr-categories)

- [Technical Tasks](#technical-tasks)  
  - [Example Technical Tasks](#example-technical-tasks)  

- [Example of a correct PBI](#example-of-a-correct-pbi)  
---


## Issues

### Issue Identification Structure

When creating an issue, it is important to follow a specific naming convention for traceability and clarity. The issue identifier should be structured as follows:

AAA-AA-###-### Short Description

* AAA: Three-letter team name
  * **SPT**  (Sprinters)
  * **CPD**  (Cprendio++)
  * **PQL**  (PreQLs)
  * **SQL**  (SQLits)

* AA: Two-letter for the functionality (to be defined for each team).
  * **LS** (Learning Spaces)

* ###: A consecutive number for the epic or feature.
  * **001** (Epic Number)

* ###: A consecutive number for the user story or task under the epic/feature.
  * **001** (PBI Number)

* Short Description: A brief description of the task or user story
  * **List Classrooms**

Example:

* **SQL-LS-001-001 List Classrooms**

### Epic Details

* **EPIC ID:** SQL-LS-001
* **Name:** Learning Spaces
* **Description:** This epic aims to design and optimize digital learning spaces to enhance the user experience by facilitating access to educational content, interactive tools, and collaborative features within the platform.

### Issue Details

When creating an issue, include the following details to ensure clarity and prioritization:

* **Story Point**: Estimate the effort required using the Fibonacci sequence: **?**, **0**, **0.5**, **1**, **2**, **3**, **5**, **8**, **13**, **20**, **100**.

* **Priority**: Define the priority level.
  * **Highest**
  * **High**
  * **Medium**
  * **Low**
  * **Lowest**

* **Risk Level**: Assess the potential risks associated with the issue. Higher values indicate greater uncertainty or potential impact on the project.
  * **High**
  * **Normal**
  * **Low**

* **Business Value**: Evaluate the importance of the issue to the business. Use values such as **50**, **100**, **200**, **300**, **600**, **800**, **1300**, **2000**, **4000**, **10000**.

* **Status**: Track the current state of the issue.
  * **New**
  * **Not Started**
  * **Assigned**
  * **In Progress**
  * **Ready for Test**
  * **Done**
  * **Accepted**
  * **Canceled**
  * **Postponed**
  * **Blocked**

* **Iteration**: Specify the iteration or sprint in which the issue is planned to be addressed.
  * Example: **Sprint 1**, **Sprint 2**, **Sprint 3**, etc.

* **Description**: Provide a detailed explanation of the issue, including its purpose and any relevant context.
  * Example: "Implement a feature to list all classrooms available in the system, including filtering options by building and capacity."

---

## User Story

User stories should follow the Gherkin structure, ensuring that they meet the INVEST criteria (**I**ndependent, **N**egotiable, **V**aluable, **E**stimable, **S**mall, **T**estable).

### User Story Format

```gherkin
Feature: [Feature Name]

  As a [type of user]  
  I want to [perform an action]  
  So that [specific goal or benefit]  
```

Example:

```gherkin
Feature: User Registration

  As a user  
  I want to register on the website  
  So that I can access the website.  
```

---

## Acceptance Criteria

Each user story should have clearly defined acceptance criteria written in Gherkin format to specify the conditions for success. This ensures that the story is testable and meets the requirements.

### Acceptance Criteria Format

```gherkin
Feature: [Feature Name]

  Scenario: [Scenario Name or Use Case]
    Given [Initial condition or context]
    When [Action performed by the user]
    Then [Expected outcome or result]
    And [Additional result, if necessary]
    But [Alternative result, if necessary]
```

Example:

```gherkin
Feature: User Registration

  Scenario: Successful Registration
    Given that the user is on the registration page
    When the user enters their full name, email address, password, and clicks on "Register"
    Then the system creates a new user account
    And redirects the user to the login page with a confirmation message.
```

### Acceptance Criteria Guidelines

* **Consider different scenarios:** Include both **positive** and **negative** test cases in the acceptance criteria

* **Be specific:** Acceptance criteria should define clear outcomes and conditions for the story to be considered complete.

* **Follow the "Given-When-Then" format:** This structure ensures that all conditions and expected outcomes are captured accurately.

---

## Non-Functional Requirements

Non-functional requirements define **system-level qualities** and **constraints** that ensure the product performs effectively under expected conditions.

### Common NFR Categories

| Category         | Description |
|------------------|-------------|
| **Performance**  | Response time, throughput, latency, etc. |
| **Scalability**  | Ability to scale up/down under load |
| **Availability** | System uptime expectations |
| **Security**     | Authentication, authorization, encryption, etc. |
| **Usability**    | Ease of use, user interface clarity |
| **Maintainability** | Ease of updating the system |
| **Portability**  | Ability to run on different environments |
| **Compliance**   | Adherence to legal or regulatory standards |
| **Reliability**  | How consistently the system behaves as expected |
| **Localization** | Support for multiple languages and regions |

---

## Technical Tasks

The technical tasks are the divisions of each PBI presented in the backlog. This practice will help us (teams) manage the work more effectively. This division is recommended to be done in a table, that way each task and their respective dedicated time can be easily identified. Each team should also include the Definition of Done (DoD) for the PBI and if necessary some assumptions.

### Example Technical Tasks

#### PBI

Me as `{the administrative head of the school}`, I want `{a list of the offered courses for the current semester}`, so that `{I can validate if all the requested courses were opened}`

**Story Points (SP):** 3

#### Technical Tasks

| Task | Minutes |
|------|---------|
| Validate basic business needs with the PO (refinement) | 20 |
| Clone repo | 5 |
| Create branch | 3 |
| Model/code the entities that provide the functionality | 40 |
| Model/code repositories in infrastructure layer (I) | 3 |
| Design and scripts for database, tables and relationships | 40 |
| Sample data (PDS) for those tables | 5 |
| Publish the database | 10 |
| Model/code repositories in infrastructure layer (C) | 30 |
| Review integration with Entity Framework (EFF) | 2 |
| Configure `bdContext` | 1 |
| Entity-to-table mappings | 15 |
| Implement listing service | 10 |
| Dependency management | 3 |
| Continuous integration to repo, merged into main + PR | 60 |
| App, Presentation, WebAPI (EP layer class) |   |
| &nbsp;&nbsp;&nbsp;App | 60 |
| &nbsp;&nbsp;&nbsp;Presentation | 10 |
| &nbsp;&nbsp;&nbsp;WebAPI | 30 |
| Build/Compilation | 5 |
| Follow Clean Code (no duplicated code, code smells, C# guidelines...) | 15 |
| Respect architecture (CA) and follow SOLID, DDD principles | 5 |
| Swagger tests | 10 |
| Automated testing: Unit, Integration, 80% coverage | 150 |
| Must be documented and status updated in backlog management tool | 15 |
| Prototyping, design, validation | 120 |
| Meet acceptance criteria, validated with the PO | 10 |

**Total estimate (minutes):** 856  
**Total estimate (hours):** 14.27

#### Definition of Done:
- Successfully compiles
- Follows Clean Code practices (no duplicated code, no code smells, respects C# guidelines, proper documentation, traceability, etc.)
- Does not break the architecture (CA) and follows SOLID, DDD principles
- Has been tested: acceptance tests, automated unit and integration tests, 80% coverage
- Is integrated into the main branch (following the established merge process)
- Has been peer-reviewed (PR)
- Is documented and its status updated in the backlog management tool
- Database is updated
- Meets the acceptance criteria validated by the PO

#### Assumptions:
- The codebase with solution and project structure is already in the main branch.
- The backlog management tool is already configured and up-to-date from refinement and planning sessions.

> [!NOTE]
> This is just an example similar to the one we studied in class, each team needs to adapt their own PBI's.


## Example of a correct PBI

### PBI: SPT-UM-001-001 View registered people

### **EPIC ID: SPT-UM-001**

**Name**: Person Management  
**Description**: As part of the core features of ThemePark@UCR, the system must allow the management of person records. This includes the ability to create, view, modify, delete, and retrieve person information. Proper person management is essential for controlling access and customizing the environment.

---

### **User Story**

```gherkin
Feature: View the registered people

As a system administrator  
I want to view a list of all registered people
So that I can manage access and ensure the correct people are in the system  
```

---

### **Acceptance Criteria**

```gherkin
Scenario: Successful view list of registered people
Given the system contains one or more registered people
When the administrator requests the person list  
Then the system should display a list of all registered people
And each entry should include basic information (e.g. name, email)  
```

```gherkin
Scenario: No registered persons
Given there are no registered peoplein the system  
When the administrator requests the person list  
Then the system should display a message indicating there are no people
And the list should be empty  
```

```gherkin
Scenario: The person list includes newly added people
Given a new person was recently registered  
When the administrator refreshes or reopens the person list  
Then the new person should appear in the list  
And their information should be displayed correctly  
```

---

### **Non-functional Requirements – View Registered People**

| Category         | Description |
|------------------|-------------|
| **Performance**  | The system should return the list of persons within 1 second under normal conditions. |
| **Reliability**  | The system should correctly reflect all registered persons 99.9% of the time. |
| **Usability**    | The list should be presented in a clear, paginated, and readable format. |
| **Maintainability** | The list-fetching logic should be decoupled and easily extendable for filtering or sorting. |
| **Portability**  | The person list feature should work across different deployment environments. |

---

### **Technical Tasks – View Registered People**

| Task | Minutes |
|------|---------|
| Confirm expected data structure with the PO | 10 |
| Ensure test database contains multiple person records | 10 |
| Implement endpoint to retrieve all person records in application layer | 35 |
| Implement corresponding method in person repository (infrastructure layer) | 25 |
| Add pagination and sorting options if applicable | 20 |
| Handle empty list case with a friendly message | 15 |
| Add Swagger documentation for the endpoint | 15 |
| Write unit and integration tests for normal and edge cases | 40 |
| Open PR and request code review | 10 |
| Confirm with PO that results meet expectations | 15 |

**Total estimate (minutes):** 195  
**Total estimate (hours):** 3.25
