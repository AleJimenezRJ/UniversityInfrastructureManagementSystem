# Git Guidelines

## Index

<!-- TOC -->

- [Git Guidelines](#git-guidelines)
  - [Index](#index)
  - [Git Workflow: Git feature branch workflow](#git-workflow-git-feature-branch-workflow)
  - [Commits](#commits)
    - [Commit types](#commit-types)
    - [Description](#description)
    - [Body](#body)
    - [Footer](#footer)
    - [Co-author](#co-author)
    - [**Commit Example**](#commit-example)
  - [Branches](#branches)
    - [Branch Types](#branch-types)
    - [Branch development team](#branch-development-team)
    - [**Branch Examples**](#branch-examples)
  - [Pull Requests](#pull-requests)
    - [Approvals](#approvals)
      - [Reviewers](#reviewers)
      - [Assignees](#assignees)
      - [**How to approve a Pull Request**](#how-to-approve-a-pull-request)
    - [**Pull Request Template**](#pull-request-template)
      - [Pull Request Description](#pull-request-description)
    - [Exceptions](#exceptions)
  - [Bibliography](#bibliography)

<!-- /TOC -->

## Git Workflow: Git feature branch workflow

The core idea behind the Feature Branch Workflow is that all feature development should take place in a dedicated branch instead of the main branch.

This encapsulation makes it easy for multiple developers to work on a particular feature without disturbing the main codebase. It also means the main branch will never contain broken code, which is a huge advantage for continuous integration environments. ([Source](https://www.atlassian.com/git/tutorials/comparing-workflows/feature-branch-workflow))

## Commits

Commit template:

```gherkin
<type>: <description> (<issue>)

<optional body>


<optional footer>
<optional co-author>
```

### Commit types

|Type|Description|
|-|-|
|`feat`|A new feature|
|`fix`|A bug fix|
|`docs`|Documentation only changes|
|`style`|Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)|
|`refactor`|A code change that neither fixes a bug nor adds a feature or deletes something|
|`perf`|A code change that improves performance|
|`test`|Adding missing tests or correcting existing tests|
|`ci`|Changes to our Continuous Integrations configuration files and scripts|
|`chore`|Other changes that don't modify src or test files|
|`reverts`|Reverts a previous commit|

### Description

- A properly formed Git commit `<description>` should always be able to complete the following sentence: **If applied, this commit will `<description>`**.
  - For example: **If applied, this commit will remove deprecated methods.**

- The first line of the commit `<type>: <description> (<issue>)` should always be in lowercase and present-tense.
  - Good example: **fix: show referrer for Wasm module dependency errors (#28653)**.
  - Bad example: **Changing behavior of Wasm module**. This commit does not follow the template, and it is in past-tense.

### Body

`<body>` is an optional field that should be used for a more detailed explanatory description of the change made in the commit. It also needs to be wrapped at about 72 characters per line.

If there is any pending work or additional consideration for the other developers, specify it at the end of the body. Include contact information of all
the developers that contributed as well as their percentage of attribution.

### Footer

If a commit fixes or closes the issue, `<footer>` should indicate a message ex: `Fixes #23`.

### Co-author

Co-authors should be added at the **end** of the commit message with the following template:

```sh
Co-authored-by: git_username <git_primary_email@example.com>
```

### Commit Example

```md
docs: add git guidelines

This commit adds the markdown file with the workflow description and templates to
be used in the repo

Fixes #2

Co-authored-by: git_username1 <git_primary_email@ucr.ac.cr>
Co-authored-by: git_username2 <git_primary_email@ucr.ac.cr>
Co-authored-by: git_username3 <git_primary_email@ucr.ac.cr>
```

## Branches

Branch template:

```gherkin
<type>/<team>/<description>
```

### Branch Types

|Type|Description|Example|
|-|-|-|
|`feature`|Used to develop new features, bug fixes or other changes in a separated branch from `main`|feature/team/add-user-authentication|
|`fix`|A bug fix applied to a *feature* branch or to the *main* branch.|fix/team/fix-buildings-read|
|`docs`|Add or edit any kind of documentation.|docs/team/git-template|
|`refactor`|A code change that neither fixes a bug nor adds a feature or deletes something|refactor/team/value-objects|

### Branch development team

A branch development **team** can refer to either a working group or a cross-functional team. If a branch has been worked on by multiple development teams, then the following acronyms should be used to specify which   name of each contributing group:

|Working Group| Acronym|
|-|-|
|Sprinters|SPT|
|#CPrendió++|CPD|
|SQLit(s)|SQL|
|Prequels|PQL|

|Cross-functional team|Acronym|
|-|-|
|Git|GIT|
|Software Quality Assurance|SQA|
|Product Backlog|PB|
|Clean Architecture|CA|
|Databases|DB|
|Communications|COM|
|Look and Feel|LF|

### Branch Examples

Example of the documentation branch where these guidelines were added:

```sh
# <type>/<team>/<description>
docs/GIT/git-guidelines
```

```sh
# <type>/<team>/<description>
feature/CA-SQA-GIT/codebase
```

## Pull Requests

### Approvals

Pull Requests should not take longer than 20 minutes to review on average. 

Keep in mind that PRs will be reviewed and merged in chronological order (First in, first out). Finally, when a PR introduces a new feature that has been worked on by only 1 working group, developers from **other** groups will be in charge of reviewing the PR and filling the template's checklist.

#### Reviewers

Each Pull Request must be reviewed by at least two developers:

- All of the reviewers should follow the [Pull Request Checklist](#pull-request-template).
- A developer from **any team** can review a PR, but make sure to follow each section's checklist and if needed, refer to both the [CA guidelines](/Guidelines/SQA-GUIDELINES.md) and [SQA Guidelines](/Guidelines/SQA-GUIDELINES.md) for more information about the project's architecture and testing.
- If available, Copilot will be used as the first automatic validation for PRs.
- All the cross-functional teams have been added to the repository as groups, so you can assign any one of them as reviewers if needed.

#### Assignees

Assignees clarify who is working on specific issues and pull requests.

#### How to approve a Pull Request

See a short tutorial on how to approve a PR [here](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/reviewing-changes-in-pull-requests/approving-a-pull-request-with-required-reviews), also see how to assign reviewers [in this link](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/requesting-a-pull-request-review). For more information about PR's, refer to the [Github documentation](https://docs.github.com/en/pull-requests).

### Pull Request Template

```git
## Description

### Type of Change

- [ ] Bug fix (**non-breaking** change which fixes an issue)
- [ ] New feature (**non-breaking** change which adds functionality)
- [ ] Documentation update

### What was done?

- Briefly explain the changes introduced.
- List new features, behavior changes, or refactors.

### How was it done?

- Technical description of the approach taken.
- Key points of the design or implementation.

### How Has This Been Tested? (Only for feature changes, not documentation)

Describe the tests that were made to verify the changes. Provide instructions so other developers can perform these tests, also list any relevant details to the test configuration.

- [ ] Test A
- [ ] Test B

### Checklists **for reviewers** (Only for feature changes, not documentation)

#### CA Checklist

- [ ] The responsibilities of each class are clearly separated according to the corresponding layer (Domain, Application, Infrastructure, Presentation).
- [ ] Interfaces and abstractions are defined in the core (Domain/Application) and implementations in external layers (Infrastructure/Presentation).
- [ ] The naming convention and coding styles defined (PascalCase, camelCase, I prefix for interfaces, etc.) are respected.
- [ ] Static analysis tools (SonarAnalyzer.CSharp, SonarAnalyzer.CSharp.Styling) have been executed and the detected problems have been corrected.

#### SQA Checklist

- [ ] The code is commented, particularly in hard-to-understand areas.
- [ ] The corresponding changes have been made to the documentation.
- [ ] The are new tests that prove the fix is effective or that the feature works.
- [ ] New and existing unit tests pass locally with my changes.

### General Checklist

- [ ] The changes generate no new warnings or **build errors**.
- [ ] Any dependent changes have been merged and published in downstream modules (Branch up to date).
- [ ] The PR does not introduce more than 500 changes.

### Additional notes

- Are there any database changes?
- Is this change backwards compatible?
```

### Exceptions

In case that a PR does not follow some of the items in the checklist, other teams must hold a discussion to determine if it should be approved or not.

## Bibliography

1. Conventional Commits cheat sheet - Kapeli. (n.d.). https://kapeli.com/cheat_sheets/Conventional_Commits.docset/Contents/Resources/Documents/index
2. Atlassian. (n.d.). Git Feature Branch Workflow | Atlassian Git Tutorial. https://www.atlassian.com/git/tutorials/comparing-workflows/feature-branch-workflow
3. Reviewing proposed changes in a pull request - GitHub Docs. (n.d.). GitHub Docs. https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/reviewing-changes-in-pull-requests/reviewing-proposed-changes-in-a-pull-request