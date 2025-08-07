# README

* University of Costa Rica

* Conceptualization Document for the ThemePark@UCR Project

* ThemePark@UCR – Web and VR Platform for University Campus Exploration

* [Version control for the project](https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi)

## Table of Contents

- [Definitions, acronyms and abbreviations](#definitions-acronyms-and-abbreviations)
- [Introduction](#introduction)
- [Teams](#teams)
  - [Prequels](#prequels)
  - [SQLit's](#sqlits)
  - [Sprinters](#sprinters)
  - [#Cprendió++](#cprendió)
- [General Description for the System](#general-description-for-the-system)
  - [Context and Current Situation](#context-and-current-situation)
  - [Solved Problem](#solved-problem)
  - [Stakeholders and Users](#stakeholders-and-users)
  - [Environment Analysis](#environment-analysis)
  - [Proposed Solution](#proposed-solution)
  - [Product Vision](#product-vision)
  - [Relationship with External Systems](#relationship-with-external-systems)
  - [Descriptions](#descriptions)
  - [Functional Requirements, Product Road Map and Non-functional Requirements](#functional-requirements-product-road-map-and-non-functional-requirements)
- [Technical Decisions](#technical-decisions)
  - [Methodologies Used and Defined Processes](#methodologies-used-and-defined-processes)
  - [Artifacts Used in Project Development](#artifacts-used-in-project-development)
  - [Technologies Used with Their Respective Versions](#technologies-used-with-their-respective-versions)
  - [Code Repository and Git Strategy for the Project](#code-repository-and-git-strategy-for-the-project)
  - [Application Architecture](#application-architecture)
  - [Definition of Done (DoD)](#definition-of-done-dod)
  - [Data Requirements](#data-requirements)
  - [Conceptual Database Design](#conceptual-database-design)
  - [Logical Database Design](#logical-database-design)

## Definitions, acronyms and abreviations

#### **General Terms**

| Acronym | Meaning                        |
| ------- | ------------------------------ |
| UCR     | University of Costa Rica       |
| VR      | Virtual Reality                |
| MVP     | Minimum Viable Product         |
| PO      | Product Owner                  |
| DoD     | Definition of Done             |

---

#### **Work Teams**

| Acronym | Work Team   |
| ------- | ----------- |
| SPT     | Sprinters   |
| CPD     | #CPrendió++ |
| SQL     | SQLit(s)    |
| PQL     | Prequels    |

---

#### **Cross-functional Teams**

| Acronym | Area                          |
| ------- | ----------------------------- |
| GIT     | Version control (Git)         |
| SQA     | Software Quality Assurance    |
| PB      | Product Backlog Management    |
| CA      | Clean Architecture            |
| DB      | Databases                     |
| COM     | Communications                |
| LF      | Look and Feel (visual design) |

---

## Introduction

#### **Objective of the document**

This document aims to present the conceptualization of the **ThemePark\@UCR** system, a Web/VR application designed to provide immersive experiences related to university life at the University of Costa Rica. It serves as a starting point to align the development team with the project’s objectives, defining its scope, involved teams, and key technical decisions for its implementation.

#### **Purpose and Structure**

The purpose is to document the fundamental aspects of the project in its initial stage: context, problem statement, user types, proposed solution, product vision, and organizational structure. Additionally, it includes assigned modules, selected technologies, system requirements, and the methodological foundation of the project.


## Teams

### Sprinters

| Name                        | Specific Role in Iteration 0            |
| --------------------------- | --------------------------------------- |
| Andrés Murillo Murillo      | Scrum Master, Quality Assurance	   	    |
| Gael Alpízar Alfaro         | Clean Architecture                      |
| Elizabeth Huang Wu          | Database Model Designer                 |
| Esteban Isaac Baires Cerdas | Ambassador, Communications, Git 		|
| Isabella Rodríguez Sánchez  | Look & Feel			    			    |
| Gypsi Tatiana Páramo López  | Product Backlog				            |

---

### Development Team: #CPrendió++

| Name                           | Specific Role in Iteration 0         |
|--------------------------------|--------------------------------------|
| Angélica Vargas Artavia        | Scrum Master, Look & Feel            |
| Ericka Araya Hidago            | Ambassador, Quality Assurance        |
| Eleni Gougani Castañeda        | Database Model Designer              |
| Luis Fonseca Chinchilla        | Git                                  |
| Sebastián Arce Flores          | Clean Architecture                   |
| Marcelo Picado Leiva           | Product Backlog                      |

---

### SQLit(s)

| Name                           | Specific Role in Iteration 0         |
|--------------------------------|--------------------------------------|
| Emmanuel Valenciano Villalobos | Scrum Master, Database Model Designer|
| Rolando Villavicencio González | Ambassador, Clean Architecture       |
| Keylor Palacios González       | Git                                  |
| Anderson Vargas Navarro        | Product Backlog                      |
| Bryan Ávila Caravaca           | Quality Assurance, Look & Feel       |

---

### Prequels

| Name                           | Specific Role in Iteration 0         |
|--------------------------------|--------------------------------------|
| Giuliana Ortega                | Scrum Master, Look & Feel            |
| Alejandro Jiménez Rojas        | Ambassador, Database Model Designer, Product Backlog |
| Felipe Quesada Parada          | Git                                  |
| Juan Diego Soto Castro         | Communications, Quality Assurance    |
| William Morales Fuentes        | Quality Assurance                    |
| Camila Fariñas Ortega          | Clean Arquitecture                   |

## General Description for the system


### Context and current situation

The University of Costa Rica seeks to innovate in how it presents itself to students and the general public through immersive technologies. Currently, exploring the campus relies on in-person visits or static websites, which limits access for many people.

Given the advancement of technologies such as Virtual Reality (VR), it is essential for the university to be prepared for these changes. Initiatives like ThemePark@UCR help address this transformation by providing a more accessible, educational, and interactive experience.

### Solved problem

Access to information about the university, its spaces, services, and academic programs is limited in terms of interactivity, dynamism, and remote availability. There are no platforms that provide an immersive, educational, and personalized experience to explore university life without being physically on campus.

### Stakeholders and users

**Stakeholders:**

* Product Owner (course instructor)
* Coordinators and professors from ECCI
* Development teams participating in the course
* University outreach office

**User Types:**

* Prospective university applicants
* Interested members of the general public
* University community (faculty, administrative staff, students)


### Proposed solution

Develop a platform called ThemePark@UCR, accessible from web browsers and VR devices, that provides an interactive, immersive, and educational experience. This solution will allow users to explore the campus and learn about its services, academic programs, spaces, and activities in an accessible and engaging way through web and virtual reality technologies.


### Environment Analysis

* **Business Strategy:** Democratize access to knowledge and university life through emerging technologies such as virtual reality and interactive web platforms.

* **System Objectives:** Provide an accessible, interactive, and immersive portal for exploring the university campus and its academic offerings.

* **Business and Customer Perspective:** Current and prospective users will be able to learn about the university independently, in an attractive and modern way, enhancing academic decision-making.

* **Expected Use:** Web or VR navigation to explore 3D maps, complete missions or gamified activities, and access key information about services and academic programs.

* **Legacy Systems:** Institutional websites, UCR maps, and guided on-site campus visits.

* **Regulatory Aspects, Constraints, and Existing Solutions:** Compliance with digital accessibility standards, personal data protection regulations, and compatibility between web and VR technologies must be ensured.


---
### Product vision

**For:**  
People interested in exploring and learning about universities, their resources, and their learning environments.

**Who:**  
Wants to access the university and its environment interactively and accessibly, to learn about or interact with its services, areas of knowledge, careers, and other available resources.

**The:**  
**ThemePark@UCR** is an **Interactive and Didactic Portal** accessible from web browsers and VR devices.

**That:**  
Allows users to explore, learn about, and interact with the university in a direct, didactic, and immersive way through the web and VR technologies.

**Unlike:**  
Other static campus representations that exclusively focus on spatial elements such as maps, websites with non-interactive content, and in-person visits to campus.

**Our Product:**  
Provides didactic, interactive, and immersive access through a portal that combines Web and VR technologies, allowing users to access from web browsers or VR devices. It offers an innovative and flexible approach that facilitates exploration, learning, and intervention in academic environments.

---
### Relationship with external systems

The system will be indirectly related to UCR’s official maps, institutional websites, and potentially to academic or event management systems in the future. It also relies on external technological platforms such as Unity, Godot, Meta Quest, and WebXR libraries to deliver immersive experiences.

---
### Descriptions

**Team Sprinters (SPT)**

* **Epic ID:** SPT-UM-001
* **Name:** Person Management
* **Description:** This epic is part of the system’s core functionalities and covers the development of the person management module. It includes the ability to create, view, update, delete, and retrieve personal information within the system. Proper management of these accounts is essential to control access, customize the user environment, and ensure a coherent experience in the ThemePark\@UCR Web/VR portal.

**Team #CPrendió++ (CPD)**

* **Epic ID:** CPD-LC-001
* **Name:** Learning components
* **Description:** This epic focuses on enhancing learning spaces within ThemePark@UCR, such as classrooms and laboratories, by adding interactive components like whiteboards and projectors. These components are designed to create an interactive and dynamic learning environment.

**Team SQLit(s) (SQL)**

* **Epic ID:** SQL-LS-001
* **Name:** Learning spaces management
* **Description:** As part of the core features of ThemePark@UCR, the system must allow users to explore and manipulate data related to learning spaces. This includes the ability to list available spaces, view details of a specific space, update existing records, create new entries, and retrieve relevant information.

**Team Prequels (PQL)**

* **Epic ID:** PQL-AB-001
* **Name:** Buildings management
* **Description:** This epic covers one of the basic features of the ThemePark@UCR, the development of the buildings module that allows users to interact with them. It includes the ability to create, list, update, and retrieve information of the buildings within the system.

---
### Functional requirements, Product Road Map and Non-functional requirements

To review the functional requirements please visit the [product backlog](https://github.com/orgs/UCR-PI-IS/projects/65) for the project.

---
## Technical decisions

### Methodologies Used and Defined Processes

The team will use the **Scrum** agile methodology, adapted to the academic environment, to manage the development of the ThemePark\@UCR system.

The project is divided into **four iterations (sprints)**:

* **Sprint 0:** Conceptualization and initial planning.
* **Sprints 1–3:** Development, testing, and integration of functionalities.

Each sprint will last approximately **3 weeks** and include the following ceremonies:

* **Daily Stand-ups:** To synchronize progress and identify blockers.
* **Sprint Planning:** Definition of tasks and objectives for each iteration.
* **Sprint Review:** Presentation of the completed functional increment.
* **Sprint Retrospective:** Review of the process and proposal of improvements.

Additionally, the following supporting practices will be applied:

* **Version control with Git**, using a branching strategy and code reviews through Pull Requests.
* **Task tracking via GitHub Projects**, organizing the backlog and sprint tasks on a Kanban board (with columns such as *To Do*, *In Progress*, *Review*, and *Done*).
* **Ongoing technical documentation**, including epics, acceptance criteria (DoD), system flows, and key decisions, all maintained in the repository and collaborative files.

### Artifacts Used in Project Development

* System conceptualization document
* User stories, epics, and product backlog
* Sprint task boards (GitHub Projects)
* Diagrams (architecture, database, etc.)
* Documented and versioned source code
* Configuration and deployment files
* Meeting logs and recorded decisions

### Technologies Used with Their Respective Versions

| Dependency Name                                      | Current Version | Source/Repository URL                                                                                  |
|------------------------------------------------------|-----------------|--------------------------------------------------------------------------------------------------------|
| Microsoft.AspNetCore.OpenApi                         |  8.0.14         | [Nuget](https://www.nuget.org/packages/Microsoft.AspNetCore.OpenApi/8.0.14)                            |
| Microsoft.Extensions.DependencyInjection.Abstractions|  9.0.3          | [Nuget](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions/9.0.3)    |
| Microsoft.Extensions.DependencyInjection             |  9.0.3          | [Nuget](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/9.0.3)                 |
| SonarAnalyzer.CSharp                                 |  10.10.0.116381 | [Nuget](https://www.nuget.org/packages/SonarAnalyzer.CSharp/10.10.0.116381)                            |
| SonarAnalyzer.CSharp.Styling                         |  10.10.0.116381 | [Nuget](https://www.nuget.org/packages/SonarAnalyzer.CSharp.Styling/10.10.0.116381)                    |


### Code Repository and Git Strategy for the Project
The team uses Git as the version control system, following a strategy based on branch naming conventions according to the type of change. Guidelines are defined for branch naming, commit formatting, and pull request handling—all governed by the official repository guide.

Branches are managed according to the purpose of the change (new feature, bug fix, documentation, etc.) and are integrated through Pull Requests, which must meet both technical and functional review criteria.

> For more details, refer to the [GIT\_GUIDELINES.md](/Docs/Guidelines/GIT-GUIDELINES.md).

### Application Architecture

The application architecture is based on the clean architecture principles proposed by Robert Martin, known as Uncle Bob. Is based on the idea of organizing the code in layers, where each layer has a specific responsibility and is independent of the others. Below is a brief overview:

**Core Principles**

- Dependency Inversion: Business logic does not depend on infrastructure or external systems.
- Domain-Driven Design (DDD): Focuses on organizing software around the business domain.
- Abstractions at the Center: Dependencies point towards the core using interfaces.

**Layers**

- Domain Layer: Contains entities and core business rules.
- Application Layer: Implements workflows, use cases, and application-specific logic.
- Infrastructure Layer: Handles external concerns like data access and APIs.
- Presentation Layer: Manages user interactions and UI logic.

**S.O.L.I.D Principles**

- Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, and Dependency Inversion are followed to ensure clean and maintainable code.

**Coding Practices**

- Consistent naming conventions (e.g., PascalCase for classes, camelCase for variables).
- Use of interfaces (I prefix) and private fields (_ prefix).
- To assist in the development of clean code, the team will use **SonarLint** tools to analyze the code and ensure adherence to best practices.

> For examples about this practices, refer to [Clean Architecture guidelines](/Docs/Guidelines/CA-GUIDELINES.md).

### Definition of Done (DoD)

A user story or task is considered “Done” when it meets all the established criteria and has been reviewed by the Software Quality Assurance (SQA) team. The criteria include:

* **Completed and approved acceptance criteria**, written in Gherkin format and covering positive, negative, and edge case scenarios. Stories must follow the INVEST principles.
* **Executed and validated tests**, including unit tests by the development team, and functional, integration, and regression tests reviewed by SQA.
* **Completed code review**, with at least two reviews: one technical (developer) and one functional (SQA). Copilot is used as the initial automatic validation.
* **Updated documentation**, including a description of the implemented functionality, test evidence, and reproduction steps.
* **Updated backlog status**, with changes reflected in the corresponding issues and task tracking board.
* **Unit and integration test requirements:** For every two people, one test class per layer must be created in unit tests, and at least two integration tests must be provided.

> For more details, refer to the [official SQA README](/Docs/Guidelines/SQA-GUIDELINES.md).

### Data Requirements

The ThemePark\@UCR system must manage structured data related to users, physical campus spaces, learning components, and academic entities. The most relevant types of data include:

* Personal information of users (students, administrative staff, visitors).
* Authentication and authorization data (users, roles, permissions).
* Representation of the campus physical structure (universities, campuses, areas, buildings, floors, learning spaces).
* Physical and virtual components within spaces (whiteboards, projectors, dimensions, locations).
* User preferences, interaction history, and personalized environment settings.

The data must comply with principles of integrity, uniqueness, and mandatory fields based on the type of entity, and must support relationships between real-world components in a Web/VR environment.

> For a complete overview of entities, attributes, relationships, and constraints, refer to the [`DB-DATA-REQUIREMENTS.md`](/Docs/Guidelines/DB-DATA-REQUIREMENTS.md) file in the official project repository.

### Conceptual Database Design


![Modelo Conceptual](./Resources/Conceptual.svg)

### Logical Database Design

![Modelo Logico](./Resources/Logical.svg)

---
