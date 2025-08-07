## **Software Quality Assurance (SQA)**

This section describes the processes and practices of Software Quality Assurance (SQA) implemented in the `ThemePark@UCR` project.

### **SQA Responsibilities**
- Validate the quality of each *Pull Request (PR)* through technical and functional reviews.
- Ensure that acceptance criteria are met as specified in each User Story (US).
- Verify that functionalities meet the *Definition of Done (DoD)*.
- Review the implementation of appropriate unit, functional, and integration tests.
- Document results and maintain traceability of tests and validations.

### **Definition of Done (DoD) - Reviewed by SQA**

For a User Story or Task to be considered *Done*, it must meet the following criteria reviewed by the SQA team:

1. **Completed and Approved Acceptance Criteria:**  
   - Each User Story must have acceptance criteria defined in Gherkin format (`Given-When-Then`).
   - Acceptance criteria must include positive scenarios, negative scenarios, and edge cases.
   - The story must adhere to the *INVEST* criteria (Independent, Negotiable, Valuable, Estimable, Small, Testable).
   - The detailed Gherkin format structure is described in the **Backlog README file**.

2. **Successfully Completed Tests:**
   - Unit Tests (Executed by the development team).
   - Functional Tests (Reviewed by SQA).
   - Integration Tests (Reviewed by SQA).
   - Regression Tests (Reviewed by SQA).

3. **Code Review:**
   - Each PR must be reviewed by two members:
     - A technical review by a developer.
     - A functional review by a member of the SQA team.
   - Copilot is used as the first automatic validation for PRs.

4. **Complete Documentation:**
   - Description of the developed functionality.
   - Evidence of tests performed (screenshots, logs, etc.).
   - Instructions for reproducing the tests.

5. **Backlog Update and Issue Status:**
   - The status of each story or task must be updated to *Done* or *Accepted* as appropriate.
   - All tests must be properly recorded in the issue tracking system (**GitHub Issues**).

---

### **Tools and Technologies Used by SQA**
- **GitHub Issues:** For managing user stories, tasks, and bugs.
- **Pull Requests (PRs):** Technical and functional review of each change before merging.
- **Copilot:** For automatic validation of PRs.
- **Unit, Functional, and Integration Tests:** Implemented and verified by SQA.
- **Documentation in Markdown (`README.md`):** Detailing acceptance criteria, test results, and current status.

---

The detailed Gherkin format structure for acceptance criteria is described in the **Backlog README file**.
