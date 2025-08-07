# SQLits Daily Scrum Logs

## Sprint 0

### Daily Scrum - 21/04/2025

| Data                | Details            |
|---------------------|--------------------|
| **Date**            | 21/04/2025      |
| **Team Name**       | SQLit(s)          |
| **Sprint #**        | 0                 |
| **Duration (minutes)**| 10 min           |

#### Member Log

| **Name**      | **What did I do yesterday?**                                                         | **What will I do today?**                                                        | **What impediments do I have?**                                                          |
|---------------|-------------------------------------------------------------------------------------|----------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------|
| Keylor        | **Pair Programming** with Anderson (**Learning Space Entity**).                     | Spike on clean architecture                                                      | We need the Floor entity.                                                               |
| Rolando       | **Pair Programming** with Bryan. Reviewing the PBI of **read-learning-spaces**. | Commit the changes for read-learning-spaces PBI.                                  | We don't fully understand the return interfaces of minimal APIs.                        |
| Anderson      | **Pair Programming** with Keylor (**Learning Space Entity**).                       | Research more about **minimal APIs**.                                            | We need the Floor entity.                                                               |
| Bryan         | **Pair Programming** with Rolando. Reviewing the topic of **read-learning-spaces** PBI. | Commit the changes for reading learning spaces.                                  | We don't fully understand the return interfaces of minimal APIs.                        |
| Emmanuel      | Programming the update-learning-spaces  PBI.                                         | Spike on minimal APIs.                                                           | The amount of tasks to complete today is overwhelming.                                  |

#### Sidebar Topics (ST)

- Integrate services.  
- Meeting on technical information about APIs.  
- Database design.  
- Coordinate the relationship between **Building -> Floor -> Learning Spaces**.
- **The connection with Learning Objects.**

---

## Daily Scrum - 28/04/2025

| Data                | Details            |
|---------------------|--------------------|
| **Date**            | 28/04/2025        |
| **Team Name**       | SQLit(s)          |
| **Sprint #**        | 0                 |
| **Duration (minutes)**| 10 min           |

| **Name**   | **What did I do yesterday?**                                                                 | **What will I do today?**                                                                 | **What impediments do I have?**                                                          |
| ---------- | ------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- |
| Anderson   | Dummy implementation of the create-learning-spaces (with Keylor).                      | Change from dummy implementation to SQL (with Keylor).                                   | We don't understand SQL very well.                                                     |
| Keylor     | Dummy implementation of the creation of Learning Spaces (with Anderson); bug fixes.         | Change from dummy implementation to SQL (with Anderson).                                 | We don't understand SQL very well.                                                     |
| Bryan      | Bug fixes with the explicit listing of learning objects; worked on the Update LearningSpace PBI. | Validate the read PBI; fix pending DB-related issues in learning spaces (with Emmanuel). | Validate with the technical assessor what the endpoint should return; the IDE doesn't clearly identify the error. |
| Rolando    | Worked on the update-learning-spaces PBI.                                                     | Validate the read PBI (with Bryan).                                                     | Validate with the technical assessor what the endpoint should return.                                   |
| Emmanuel   |    Worked on the Update LearningSpace PBI.                                                                                         | Fix pending DB-related issues in learning spaces (with Bryan).                           | The database is not yet public and formalized with the remote server.                  |

#### Side Bar Topics (ST)

- Coordinate with the learning elements and buildings team.
- Coordinate databases.

---


## Daily Scrum - 30/04/2025

| Data                | Details            |
|---------------------|--------------------|
| **Date**            | 30/04/2025      |
| **Team Name**       | SQLit(s)          |
| **Sprint #**        | 0                 |
| **Duration (minutes)**| 10 min           |

#### Member Log

| **Name**   | **What did I do yesterday?**                                       | **What will I do today?**                                                       | **What impediments do I have?**                                                        |
| ---------- | ----------------------------------------------------------------- | ------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------- |
| Emmanuel   | Commits for the PBI update-learning-spaces PBI.                         | Formalize the creation of the project's database tables.                         | Disorganization among teams and lack of knowledge on how to publish to a remote DB.     |
| Rolando    | Integrations into the main branch for the PBI of read and update with teammates | Request a Pull Request to integrate into the main branch.                        | Doubts about the design of the identity tables.                                         |
| Keylor     | Merge with Anderson into the main branch                          | Define SQL tables and the activity for the retrospective (together with Anderson) | Does not fully understand how to connect SQL with C#.                                   |
| Anderson   | Pull Request for the feature of creating learning spaces          | Define SQL tables and the activity for the retrospective (together with Keylor). | Lack of knowledge about SQL and its integration with Visual Studio.                     |
| Bryan      | Implementation of the Read for Learning Spaces with dummy data.    | Meeting with the SQLit(s) team about sprint 0 pending tasks.                     | Unclear on how the retrospective will be planned                                       |

#### Side Bar Topics (ST)

* Organization and validation with other teams regarding the database design.
* Lack of knowledge in testing.

---

## Daily Scrum - 05/05/2025

| Data                | Details            |
|---------------------|--------------------|
| **Date**            | 05/05/2025        |
| **Team Name**       | SQLit(s)          |
| **Sprint #**        | 0                 |
| **Duration (minutes)**| 10 min           |

| **Name**   | **What did I do yesterday?**                                           | **What will I do today?**                                      | **What impediments do I have?**                                  |
| ---------- | --------------------------------------------------------------------- | ------------------------------------------------------------- | ---------------------------------------------------------------- |
| Rolando    | Implemented validation with Value Objects in the update-learning spaces PBI | Finish defining the retrospective activity with my teammates.   | Need to validate with the PO the topic of the internal Id with Identity and the name. |
| Emmanuel   | Published the database on the Azure server.                            | Finish defining the retrospective activity with my teammates.   | Need to validate with the PO the topic of the internal Id with Identity and the name. |
| Keylor     | Added validation with Value Objects (together with Anderson).          | Review sprint 1 issues (together with Anderson).               | Uncertainty about whether what was done is correct.              |
| Anderson   | Added validation with Value Objects (together with Keylor).            | Review sprint 1 issues (together with Keylor).                 | Uncertainty about whether what was done is correct.              |
| Bryan      | Refactored the list-learning-spaces PBI.                            | Finish defining the retrospective activity with my teammates.   | Need to validate with the PO the topic of the internal Id with Identity and the name. |

**Side Bar Topics (ST)**

- Integration of each team's pieces for mapping from DTO to Entity and from Entity to DTO in the `validations-and-exceptions` branch.

---

## Daily Scrum - 06/05/2025

| Data                | Details            |
|---------------------|--------------------|
| **Date**            | 06/05/2025      |
| **Team Name**       | SQLit(s)          |
| **Sprint #**        | 0                 |
| **Duration (minutes)**| 10 min           |

#### Member Log

| **Name**  | **What did I do yesterday?**                                | **What will I do today?**                        | **What impediments do I have?**               |
| ----------| ---------------------------------------------------------- | ------------------------------------------------ | --------------------------------------------- |
| Keylor    | Mapping of the Create PBI and refactoring.                  | Verify the keys of the internal database.         | Impact on other Scrum teams.                   |
| Emmanuel  | Mapping of the Read PBI and refactoring.                    | Plan the exceptions class.                        | Validation and integration with other teams.   |
| Anderson  | Mapping of the Create PBI and refactoring (together with Keylor). | Verify the keys of the internal database.         | Impact on other Scrum teams.                   |
| Bryan     | Mapping of the Listing PBI and refactoring.                 | Unify the code among teams.                       | Disorganization among teams                   |
| Rolando   | Mapping of the Update PBI and refactoring.                  | Plan the exceptions class.                        | Validation and integration with other teams.   |

#### Side Bar Topics (ST)

- Validate the flow of functionalities between teams and coordinate the review.

