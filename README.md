# What is it?
- Provides an abstraction that encapsulates data access, making your code testable, reusable as well as maintainable
- It adds another layer between the data consumer/requester and the logic for fetching the desired data
  ![[Pasted image 20220116184135.png]]

# Why use it?
- Normal pattern of accessing data:
  ![[Pasted image 20220116184229.png]]
- The issue with this approach is that:
    - The controller is tightly coupled with the data access layer. So the controller uncecessarily has to know how we are using the ORM and the database.
    - It's difficult to write a test for the controller without side effects.
    - Hard to exten entities with domain specific behaviour before they are returned back to the consumer.

# Benefits of the Repository Pattern
- The consumer (controller) is now seperated (decoupled) from the data access.
- Easy to write a test without side-effects.
- Modify and extend entities before they are passed on to the consumer is now possible.
- Provides a sharable abstraction resulting in less duplication of code.

# How to implement
- We want to decouple data access from controllers
- Create Infrastructure Class Library
    - Add `Repositories` folder
- Inside the folder create `IRepository` [[interface]] which will be used to dictate a contract with the repository.
    - This interface is in charge of the CRUD operations we want to perform with the data.
- Now you can either define a repository (an [[abstract]] class) in the same folder that implements `IRepository ` for each model you want to enable data accessibility. Or instead to keep things [[DRY]], implement a **generic repository**.
- A generic repository
    - This provides the benefit that we only need one repository, rather then needing multiple (each one for each model).
        - After defining a generic repository, just use [[inheritance]] for each required model.
    - This generic repo implements the base functionality defined in `IRepository` .
    - If a particular model requires its own repository later on because we want to introduce more operations for its data, then we can subclass of the generic repository.
        - These subclasses can add **custom behaviour** to each of the methods **before data** is passed to the **database or returned** to the caller.
    - The consumer (controller) will now have a **generic** way to work with data in the database **without knowing** about what database tech/ORM we are using.
- Defining `GenericRepository.cs`
    - Define in Repositories folder.
    - Define as [[abstract class]] that implements `IRepository`.
        - Inside this class, we only want to work with reference types i.e. classes, so add `where T : class` .
        - Auto-complete should implement `IRepository` 's methods in this class. If not, you need to add manually.
    - Now, only the repositories should know about the `DbContext` and how to interact with the database.
        - Add the `DbContext` to the `GenericRepository` class and initialise it in the class contructor.
    - Add the required definitions for the [[generic methods]] in the `GenericRepository.cs` of how to work with the database.
        - For example `Add` method, return the entity after its been added to the context.
        - If you want to create subclasses for additional functionality, you will need to make these [[virtual methods]].
- 