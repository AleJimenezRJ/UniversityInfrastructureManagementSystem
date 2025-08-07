# Clean Architecture

## Table of Contents

1. [What is it?](#what-is-it)
2. [Fundamental Principles](#fundamental-principles)
3. [Visual Structure](#visual-structure)
4. [Dependency Injection](#dependency-injection)
   - [Advantages of Dependency Separation](#advantages-of-dependency-separation)
5. [Clean Architecture Layers](#clean-architecture-layers)
   - [Domain Layer](#domain-layer)
   - [Application Layer](#application-layer)
   - [Infrastructure](#infrastructure)
   - [Presentation Layer](#presentation-layer)
6. [Guidelines to Adhering with S.O.L.I.D Principles](#guidelines-to-adhering-with-solid-principles)
   - [1. Does this class do only one thing?](#1-does-this-class-do-only-one-thing)
   - [2. Can I add functionality without changing this?](#2-can-i-add-functionality-without-changing-this)
   - [3. Could I replace this class with a similar one?](#3-could-i-replace-this-class-with-a-similar-one)
   - [4. Is this interface too large?](#4-is-this-interface-too-large)
   - [5. Am I depending on an interface and not a concrete class?](#5-am-i-depending-on-an-interface-and-not-a-concrete-class)
7. [Coding Practices](#coding-practices)
8. [Static Code Analysis Tools](#static-code-analysis-tools)
9. [Bibliography](#bibliography)

## What is it?

**Clean Architecture** is a software design approach that aims to clearly separate responsibilities within an application, promoting independence between layers. It is an evolution of other architectures such as:

- Hexagonal Architecture
- Ports and Adapters
- Onion Architecture

## Fundamental Principles

- **Dependency Inversion**: In this architecture, business logic and the application model do not depend on data access or other infrastructure concerns. This dependency is inverted: infrastructure and implementation details depend on the Application Core.
- **Domain-Driven Design (DDD)**: Organizes software around the business domain.
- **Abstractions at the center**: Dependencies point towards the core, using **interfaces** implemented by external layers.

## Visual Structure

Below is a graphical representation of Clean Architecture:

![Clean Architecture Onion View](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/media/image5-7.png)

The architecture can be represented as a series of **concentric circles**, where:

- The center is the **domain model** (business rules).
- Outer layers are responsible for use cases, infrastructure, UI, databases, etc.
- Outer layers **depend** on inner layers, but **not vice versa**.

In Clean Architecture, the **dependency flow always points towards the core** of the application. This design ensures that:

- The **core** (entities and application interfaces) **does not depend on any other layer**.
- **Domain services**, while outside the strict core, still form part of the central logic and typically implement core interfaces.
- External layers like **infrastructure** and **presentation** may depend on the core but **not necessarily on each other**.

This approach promotes independence, reusability, and **ease of testing**.

![Ease of testing](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/media/image5-10.png)

> Because the Application Core doesn’t depend on Infrastructure, it’s very easy to write automated unit tests for this layer.

## Dependency Injection

![Clean Architecture Layers View](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/media/image5-8.png)

With Clean Architecture, the **user interface** works directly with the **interfaces defined in the core** of the application and shouldn't know about implementation types defined in the Infrastructure layer. However, these implementation types are required for the app to execute, so they need to be present and wired up to the Application Core interfaces via **dependency injection**.

### Advantages of Dependency Separation

Since the **user interface does not directly depend on the infrastructure**, it is easy to:

- **Replace implementations** as requirements change.
- **Facilitate testing**, using mock or dummy implementations.
- **Keep the application decoupled** and modular.

## Clean Architecture Layers

In a solution based on Clean Architecture, each **project** has well-defined responsibilities. Types and classes are organized into corresponding folders within their respective projects.

---

### Domain Layer

It holds the business rules, which includes:

- **Entities**: Business model classes that are persisted.
- **Interfaces**: Include abstractions for operations that will be performed using Infrastructure.

---

### Application Layer

Implements workflows, use cases, or specific services of the application layer related to the business logic:

- **Use Cases**: Contains rules and business logic, orchestrating interactions between entities and data sources.
- **Interfaces**: Include abstractions for operations that will be performed using Infrastructure.

---

### Infrastructure

Typically includes data access implementations and services related to infrastructure concerns, which implement the interfaces defined in the core. That's why the Infrastructure should have a reference to the Application Core project. Its main purpose is to **connect the core with external resources**. This is accomplished by the use of **repositories**, which host data access implementations.

> Remember: The infrastructure **depends on the core**, but not vice versa.

---

### Presentation Layer

This is the **entry point** for the application, responsible for managing the logic and interactions of the user interface. This project should reference the application core too, and its types should only interact with infrastructure through interfaces.

- **Controllers**
- **Custom Filters**
- **Custom Middleware**
- **Views**
- **ViewModels**

> The presentation layer **should only depend on the core** and use its interfaces. It should not create direct instances or use static calls to the infrastructure.

---

#### And how do we achieve that?

To connect concrete implementations with their interfaces during application startup, the **presentation** project may need to reference the **infrastructure** project, but this approach is not recommended. This dependency can be eliminated with the use of a **custom dependency injection container** with support for:

- **Automatic loading** of implementations from assemblies.
- Greater **decoupling** between presentation and infrastructure.

An example of the folder structure visualization in Clean Architecture:

```bash
IntegratedProject/
├── src/
│   ├── Application/           # Application Layer (Core Business Logic)
│   ├── Domain/                # Domain Layer (Entities and Core Business Rules)
│   ├── Infrastructure/        # Infrastructure Layer (External Implementations)
│   ├── Presentation/          # Presentation Layer (Web/VR Frontend)
│   └── Shared/                # Shared Components Across Layers
├── tests/                     # Unit and Integration Tests (Can be next to each layer)
└── docs/                      # Project Documentation
```

## A few more things

### Guidelines for adhering to S.O.L.I.D principles
You can ask yourself these questions to guide you through the process of writing better code:

#### 1. **Does this class do only one thing?** (Single Responsibility Principle)

The **Single Responsibility Principle** states that a class should have only one reason to change. In other words, it should have only one responsibility.

##### What does it mean?

A class should have a single task or function in the system. If a class does more than one thing, it can become difficult to maintain, as changes to one responsibility may unnecessarily affect other parts of the code.

#### 2. **Can I add functionality without changing this?** (Open/Closed Principle)

The **Open/Closed Principle** states that software should be **open for extension but closed for modification**. That is, you should be able to add new functionality without modifying existing code.

##### What does it mean?

When you want to add new functionality, you should not modify existing classes or modules that are already working correctly. Instead, extend the code so you can add new features without altering the classes already in use.

#### 3. **Could I replace this class with a similar one?** (Liskov Substitution Principle)

The **Liskov Substitution Principle** states that objects of a derived class must be able to replace objects of the base class without altering the correct behavior of the program.

##### What does it mean?

When a class inherits from another, it must be able to substitute it without breaking anything. Subclasses should be usable wherever a base class is expected, without breaking the system or affecting behavior.

#### 4. **Is this interface too large?** (Interface Segregation Principle)

The **Interface Segregation Principle** is based on the idea of not forcing clients to implement methods they do not need, achieved through the use of smaller, more specific interfaces.

##### What does it mean?

An interface should be as small and specific as possible. If an interface is too large, classes implementing it will have to implement methods irrelevant to them, leading to higher coupling and greater difficulty in maintaining the code.

#### 5. **Am I depending on an interface and not a concrete class?** (Dependency Inversion Principle)

The **Dependency Inversion Principle** states that high-level modules should not depend on low-level modules. Both should depend on abstractions (interfaces).

##### What does it mean?

Instead of a class depending directly on a concrete class, it should depend on an interface or abstract class. This makes the code more flexible and easier to modify.

### Coding Practices

Below are some guidelines with examples:

- **PascalCase**: Use PascalCase for naming constants, classes, methods, and properties.

    ```csharp
    // Constants
    public const int MaxRetryCount = 5;

    // Classes
    public class UserManager { }

    // Methods
    public void ProcessOrder() { }

    // Properties
    public string FirstName { get; set; }

    ```

- **CamelCase**: Use camelCase for naming local variables, method parameters, and private fields.

    ```csharp
    // Local variables
    int retryCount = 0;
    string userName = "JohnDoe";

    // Method parameters
    public void UpdateUser(string userName, int userId) { }
    ```

- **Private attributes**: Prefix private fields with an underscore _ to distinguish them from local variables or method parameters.
>Even with the underscore, private fields should still follow camelCase naming convention.

```csharp
    public class User
    {
        private string _firstName;
        private string _lastName;

        public User(string firstName, string lastName)
        {
            _firstName = firstName;
            _lastName = lastName;
        }
    }
 ```

- **Interfaces**: Prefix interface names with an `I` to clearly identify them.

>Despite the I prefix, interfaces should still follow PascalCase naming convention.

```csharp
    public interface IUserRepository
    {
        void AddUser(User user);
        User GetUserById(int id);
    }

    public class UserRepository : IUserRepository
    {
        public void AddUser(User user) { /* Implementation */ }
        public User GetUserById(int id) { /* Implementation */ return new User(); }
    }
```

You must follow these practices to keep your code consistent and easier to read.


### Static Code Analysis Tools

We'll be using the following tools for static code analysis:

- **SonarAnalyzer.CSharp**: Provides comprehensive static analysis for C# code, helping to identify bugs, vulnerabilities, and code smells.  
  [SonarAnalyzer.CSharp on NuGet](https://www.nuget.org/packages/SonarAnalyzer.CSharp)

- **SonarAnalyzer.CSharp.Styling**: Focuses on enforcing consistent coding styles and best practices to improve code readability and maintainability.  
  [SonarAnalyzer.CSharp.Styling on NuGet](https://www.nuget.org/packages/SonarAnalyzer.CSharp.Styling)

Both tools can be installed via the NuGet Package Manager for the solution of the project.

## Bibliography

### 1. Microsoft. (n.d.). *Modern Web Application Architectures in Azure*. Retrieved from [https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)

### 2. DrunknCode. (2021). *Clean Architecture Simplified and In-Depth Guide*. Medium. Retrieved from [https://medium.com/@DrunknCode/clean-architecture-simplified-and-in-depth-guide-026333c54454](https://medium.com/@DrunknCode/clean-architecture-simplified-and-in-depth-guide-026333c54454)

### 3. Bimar Teknoloji. (2021). *Understanding Clean Architecture and Domain Driven Design (DDD)*. Medium. Retrieved from [https://medium.com/bimar-teknoloji/understanding-clean-architecture-and-domain-driven-design-ddd-24e89caabc40](https://medium.com/bimar-teknoloji/understanding-clean-architecture-and-domain-driven-design-ddd-24e89caabc40)
