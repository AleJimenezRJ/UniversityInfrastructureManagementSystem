using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using FluentAssertions;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations.ComponentManagement;

/*
User Stories: The testing for this class corresponds to EPIC ID: CPD-LC-001 and its PBIs #9, #23 and #180.

Technical tasks performed:

- Implemented various unit tests according to the different methods for the application services
- Proper XML documentation added
- Handle valid and invalid parameters and validations
- Pair programming with the team to ensure proper implementation of the business logic
- Validation with the Product Owner to verify the prioritized tasks
- Consultation with technical assistants to ensure the correct implementation of the tests

Participants:
    - Ericka Araya Hidalgo. C20553
    - Luis Fonseca Chinchilla. C03035
*/


/// <summary>
/// Unit test class used to verify the accuracy, robustness, and reliability of the
/// LearningComponentServices implementation. The test cases ensure proper behaviors
/// and interactions with dependencies such as the learning component repository
/// while validating key service functionalities critical to expected business logic.
/// </summary>
public class LearningComponentServiceTests
{
    /// <summary>
    /// Mock instance of <see cref="ILearningComponentRepository"/> used for unit testing.
    /// Represents the repository responsible for managing learning components in the system.
    /// Injected into the service under test to simulate repository interactions and control test scenarios.
    /// </summary>
    private readonly Mock<ILearningComponentRepository> _learningComponentRepositoryMock;

    /// <summary>
    /// Instance of <see cref="LearningComponentServices"/> used as the primary subject of unit testing.
    /// Represents the service responsible for handling operations related to learning components in the system.
    /// </summary>
    private readonly LearningComponentServices _serviceUnderTest;

    /// <summary>
    /// Instance of <see cref="Whiteboard"/> representing a single whiteboard entity used in unit tests.
    /// Serves as a mock instance retrieved from the repository to verify service behavior and ensure correct processing of whiteboard data.
    /// </summary>
    private readonly Whiteboard _whiteboardToGet;

    /// <summary>
    /// Represents an empty collection of <see cref="LearningComponent"/> used in test scenarios
    /// where no learning components are expected or returned.
    /// This is initialized as an empty list to simulate cases with no data in the repository.
    /// </summary>
    private readonly PaginatedList<LearningComponent> _emptyLearningComponentList;

    /// <summary>
    /// Represents a collection of <see cref="LearningComponent"/> instances used in unit test scenarios.
    /// Serves as a predefined list of components initialized with example data for testing purposes.
    /// </summary>
    private readonly List<LearningComponent> _learningComponentList;

    /// <summary>
    /// Represents a collection of <see cref="LearningComponent"/> instances used in unit test scenarios.
    /// Serves as a predefined list of whiteboards initialized with example data for testing purposes.
    /// </summary>
    private readonly List<LearningComponent> _whiteboardList;

    /// <summary>
    /// Represents a collection of <see cref="LearningComponent"/> instances used in unit test scenarios.
    /// Serves as a predefined list of projectors initialized with example data for testing purposes.
    /// </summary>
    private readonly List<LearningComponent> _projectorList;

    /// <summary>
    /// Represents a collection of <see cref="LearningComponent"/> instances used in unit test scenarios.
    /// Serves as a predefined list of north oriented components initialized with example data for testing purposes.
    /// </summary>
    private readonly List<LearningComponent> _northList;

    /// <summary>
    /// Represents a collection of <see cref="LearningComponent"/> instances used in unit test scenarios.
    /// Serves as a predefined list of south oriented components initialized with example data for testing purposes.
    /// </summary>
    private readonly List<LearningComponent> _southList;

    /// <summary>
    /// Represents a collection of <see cref="LearningComponent"/> instances used in unit test scenarios.
    /// Serves as a predefined list of east oriented components initialized with example data for testing purposes.
    /// </summary>
    private readonly List<LearningComponent> _eastList;

    /// <summary>
    /// Represents a collection of <see cref="LearningComponent"/> instances used in unit test scenarios.
    /// Serves as a predefined list of west oriented components initialized with example data for testing purposes.
    /// </summary>
    private readonly List<LearningComponent> _westList;

    /// <summary>
    /// Instance of <see cref="PaginatedList{LearningComponent}"/> used to represent a paginated list of learning components.
    /// Provides pagination functionality for managing a portion of the full list of <see cref="LearningComponent"/> objects in the unit tests.
    /// Used in the test cases to validate service methods that rely on paginated data handling.
    /// </summary>
    private readonly PaginatedList<LearningComponent> _paginatedList;

    /// <summary>
    /// Instance of <see cref="PaginatedList{LearningComponent}"/> used to represent a paginated list of learning components.
    /// Provides pagination functionality for managing a portion of the full list of <see cref="LearningComponent"/> objects in the unit tests.
    /// Used in the test cases to validate service methods that rely on paginated data handling.
    /// </summary>
    private readonly PaginatedList<LearningComponent> _paginatedWhiteboardList;

    /// <summary>
    /// Instance of <see cref="PaginatedList{LearningComponent}"/> used to represent a paginated list of learning components.
    /// Provides pagination functionality for managing a portion of the full list of <see cref="LearningComponent"/> objects in the unit tests.
    /// Used in the test cases to validate service methods that rely on paginated data handling.
    /// </summary>
    private readonly PaginatedList<LearningComponent> _paginatedProjectorList;

    /// <summary>
    /// Instance of <see cref="PaginatedList{LearningComponent}"/> used to represent a paginated list of learning components.
    /// Provides pagination functionality for managing a portion of the full list of <see cref="LearningComponent"/> objects in the unit tests.
    /// Used in the test cases to validate service methods that rely on paginated data handling.
    /// </summary>
    private readonly PaginatedList<LearningComponent> _paginatedNorthList;

    /// <summary>
    /// Instance of <see cref="PaginatedList{LearningComponent}"/> used to represent a paginated list of learning components.
    /// Provides pagination functionality for managing a portion of the full list of <see cref="LearningComponent"/> objects in the unit tests.
    /// Used in the test cases to validate service methods that rely on paginated data handling.
    /// </summary>
    private readonly PaginatedList<LearningComponent> _paginatedSouthList;

    /// <summary>
    /// Instance of <see cref="PaginatedList{LearningComponent}"/> used to represent a paginated list of learning components.
    /// Provides pagination functionality for managing a portion of the full list of <see cref="LearningComponent"/> objects in the unit tests.
    /// Used in the test cases to validate service methods that rely on paginated data handling.
    /// </summary>
    private readonly PaginatedList<LearningComponent> _paginatedEastList;

    /// <summary>
    /// Instance of <see cref="PaginatedList{LearningComponent}"/> used to represent a paginated list of learning components.
    /// Provides pagination functionality for managing a portion of the full list of <see cref="LearningComponent"/> objects in the unit tests.
    /// Used in the test cases to validate service methods that rely on paginated data handling.
    /// </summary>
    private readonly PaginatedList<LearningComponent> _paginatedWestList;

    /// <summary>
    /// Represents the default number of items per page used in pagination for tests
    /// within the LearningComponentServiceTests class.
    /// It ensures consistent handling of paginated data scenarios across unit tests.
    /// </summary>
    private readonly int _pageSize;

    /// <summary>
    /// Represents the index of the current page used for pagination or iteration during unit tests
    /// related to learning component service functionalities.
    /// Helps in validating scenarios involving paginated data retrieval.
    /// </summary>
    private readonly int _pageIndex;

    /// <summary>
    /// The string to filter the results on.
    /// </summary>
    private readonly string _stringSearch;

    /// <summary>
    /// Unit test class designed to validate the functionality of the LearningComponentServices
    /// implementation. It includes various test cases to ensure the proper interaction
    /// between the service and the repository, along with verifying business logic execution.
    /// </summary>
    public LearningComponentServiceTests()
    {
        _pageSize = 10;
        _pageIndex = 0;
        _stringSearch = string.Empty;
        _learningComponentRepositoryMock = new Mock<ILearningComponentRepository>();
        _serviceUnderTest = new LearningComponentServices(_learningComponentRepositoryMock.Object);
        _emptyLearningComponentList = PaginatedList<LearningComponent>.Empty(_pageSize, _pageIndex);
        _learningComponentList =
        [
            new Whiteboard
            {
                MarkerColor = Colors.Create("blue"),
                ComponentId = 1,
                Orientation = Orientation.Create("North"),
                Dimensions = Dimension.Create(120, 80, 2),
                Position = Coordinates.Create(3, 10, 20),
            },

            new Projector
            {
                ComponentId = 2,
                Dimensions = Dimension.Create(120, 80, 2),
                Position = Coordinates.Create(3, 10, 20),
                Orientation = Orientation.Create("North"),
                ProjectedContent = "Lecture video",
                ProjectionArea = Area2D.Create(120, 80),
            }
        ];

        _whiteboardList =
        [
            new Whiteboard
            {
                MarkerColor = Colors.Create("blue"),
                ComponentId = 1,
                Orientation = Orientation.Create("North"),
                Dimensions = Dimension.Create(120, 80, 2),
                Position = Coordinates.Create(3, 10, 20),
            }
        ];

        _projectorList = 
        [
            new Projector
            {
                ComponentId = 2,
                Dimensions = Dimension.Create(120, 80, 2),
                Position = Coordinates.Create(3, 10, 20),
                Orientation = Orientation.Create("North"),
                ProjectedContent = "Lecture video",
                ProjectionArea = Area2D.Create(120, 80),
            }
        ];

        _northList =
        [
            new Whiteboard
            {
                MarkerColor = Colors.Create("blue"),
                ComponentId = 1,
                Orientation = Orientation.Create("North"),
                Dimensions = Dimension.Create(120, 80, 2),
                Position = Coordinates.Create(3, 10, 20),
            }
        ];
        _southList =
        [
            new Projector
            {
                ComponentId = 2,
                Dimensions = Dimension.Create(120, 80, 2),
                Position = Coordinates.Create(3, 10, 20),
                Orientation = Orientation.Create("South"),
                ProjectedContent = "Lecture video",
                ProjectionArea = Area2D.Create(120, 80),
            }
        ];
        _westList =
        [
            new Whiteboard
            {
                MarkerColor = Colors.Create("blue"),
                ComponentId = 3,
                Orientation = Orientation.Create("West"),
                Dimensions = Dimension.Create(120, 80, 2),
                Position = Coordinates.Create(3, 10, 20),
            }
        ]; 
        _eastList =
        [
            new Projector
            {
                ComponentId = 4,
                Dimensions = Dimension.Create(120, 80, 2),
                Position = Coordinates.Create(3, 10, 20),
                Orientation = Orientation.Create("East"),
                ProjectedContent = "Lecture video",
                ProjectionArea = Area2D.Create(120, 80),
            }
        ];

        _whiteboardToGet = new Whiteboard
        {
            MarkerColor = Colors.Create("blue"),
            ComponentId = 1,
            Orientation = Orientation.Create("North"),
            Dimensions = Dimension.Create(120, 80, 2),
            Position = Coordinates.Create(3, 10, 20),
        };

        _paginatedList = new PaginatedList<LearningComponent>(_learningComponentList, _learningComponentList.Count, _pageSize, _pageIndex);
        _paginatedWhiteboardList = new PaginatedList<LearningComponent>(_whiteboardList, _whiteboardList.Count, _pageSize, _pageIndex);
        _paginatedProjectorList = new PaginatedList<LearningComponent>(_projectorList, _projectorList.Count, _pageSize, _pageIndex);
        _paginatedNorthList = new PaginatedList<LearningComponent>(_northList, _northList.Count, _pageSize, _pageIndex);
        _paginatedSouthList = new PaginatedList<LearningComponent>(_southList, _southList.Count, _pageSize, _pageIndex);
        _paginatedEastList = new PaginatedList<LearningComponent>(_eastList, _eastList.Count, _pageSize, _pageIndex);
        _paginatedWestList = new PaginatedList<LearningComponent>(_westList, _westList.Count, _pageSize, _pageIndex);

    }

    /// <summary>
    /// Confirms that the method retrieves and returns a learning component when the specified identifier
    /// exists in the repository, ensuring that the correct entity is returned.
    /// </summary>
    /// <returns>A task representing the asynchronous test. Upon completion, verifies that the method returns the expected learning component.</returns>
    [Fact]
    public async Task GetSingleLearningComponentByIdAsync_ShouldReturnLearningComponent_WhenIdExists()
    {
        // Arrange
        int id = 1;
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetSingleLearningComponentAsync(id))
            .ReturnsAsync(_whiteboardToGet);
        // Act
        var learningComponent = await _serviceUnderTest.GetSingleLearningComponentByIdAsync(id);

        // Assert
        learningComponent.Should().BeSameAs(_whiteboardToGet,
            because: "service should forward whatever the repository returns");
    }

    /// <summary>
    /// Validates that the method returns null when the specified identifier does not exist
    /// in the repository, indicating that no learning component matches the criteria.
    /// </summary>
    /// <returns>A task representing the asynchronous test. Upon completion, verifies that the method returns null.</returns>
    [Fact]
    public async Task GetSingleLearningComponentByIdAsync_ShouldNull_WhenIdDoesNotExist()
    {
        // Arrange
        int id = -1;
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetSingleLearningComponentAsync(id))
            .ReturnsAsync((LearningComponent?)null);

        // Act
        var result = await _serviceUnderTest.GetSingleLearningComponentByIdAsync(id);

        // Assert
        result.Should().BeNull(because: "repository returns null when component does not exist");
    }

    /// <summary>
    /// Validates that the method retrieves the correct list of learning components
    /// when the specified identifier exists in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous test. Upon completion, verifies that the expected list of learning components is returned.</returns>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_ShouldReturnLearningComponents_WhenIdExists()
    {
        int learningSpaceId = 1;
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, _stringSearch)
            )
            .ReturnsAsync(_paginatedList);

        // Act
        var learningComponents = await _serviceUnderTest.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, _stringSearch);
        
        // Assert
        learningComponents.Should().BeSameAs(_paginatedList,
            because: "service should forward whatever the repository returns");
    }

    /// <summary>
    /// Validates that the method returns an empty list of learning components
    /// when the specified identifier does not exist in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous test. Upon completion, verifies that an empty list is returned.</returns>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_ShouldReturnEmptyList_WhenIdDoesNotExist()
    {
        int learningSpaceId = -1;
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, _stringSearch)
            )
            .ReturnsAsync(_emptyLearningComponentList);

        // Act
        var learningComponents = await _serviceUnderTest.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, _stringSearch);
        
        // Assert
        learningComponents.Should().BeSameAs(_emptyLearningComponentList, because: "repository returns empty list when component does not exist");
    }

    /// <summary>
    /// Validates that the method returns true when a learning component
    /// with the specified identifier exists and is successfully deleted.
    /// </summary>
    /// <returns>A task representing the asynchronous test. Upon completion, verifies that the method returns true.</returns>
    [Fact]
    public async Task DeleteLearningComponentAsync_ShouldReturnTrue_WhenIdExists()
    {
        // Arrange
        int id = 1;
        _learningComponentRepositoryMock
            .Setup(repo => repo.DeleteComponentAsync(id)
            )
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.DeleteLearningComponentAsync(id);

        // Assert
        result.Should().BeTrue(because: "repository returns true when component is deleted");
    }

    /// <summary>
    /// Verifies that the method returns false when attempting to delete a learning component
    /// with an identifier that does not exist in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous test. Upon completion, asserts that false is returned.</returns>
    [Fact]
    public async Task DeleteLearningComponentAsync_ShouldReturnFalse_WhenIdDoesNotExist()
    {
        // Arrange
        int id = -1;
        _learningComponentRepositoryMock
            .Setup(repo => repo.DeleteComponentAsync(id)
            )
            .ReturnsAsync(false);

        // Act
        var result = await _serviceUnderTest.DeleteLearningComponentAsync(id);

        // Assert
        result.Should().BeFalse(because: "repository returns false when component is not deleted");
    }

    /// <summary>
    /// Ensures that the service retrieves a list of all learning components
    /// using the repository with proper pagination parameters.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous test. Upon completion, 
    /// verifies that the method returns the expected collection of learning components.
    /// </returns>
    [Fact]
    public async Task GetLearningComponentAsync_ShouldReturnPaginatedComponents()
    {
        // Arrange
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetAllAsync(_pageSize, _pageIndex))
            .ReturnsAsync(_learningComponentList);

        // Act
        var result = await _serviceUnderTest.GetLearningComponentAsync(_pageSize, _pageIndex);

        // Assert
        result.Should().BeEquivalentTo(_learningComponentList,
            because: "the service should return the components provided by the repository");
    }

    /// <summary>
    /// Ensures that the service returns an empty list when no learning components exist
    /// </summary>
    /// <returns>
    /// An empty list
    /// </returns>
    [Fact]
    public async Task GetLearningComponentsAsync_ShouldReturnEmptyList_WhenNoComponentsExist()
    {
        // Arrange
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetAllAsync(_pageSize, _pageIndex))
            .ReturnsAsync(_emptyLearningComponentList);
        // Act
        var result = await _serviceUnderTest.GetLearningComponentAsync(_pageSize, _pageIndex);
        // Assert
        result.Should().BeEmpty(because: "the repository returns an empty list when no components exist");
    }

    /// <summary>
    /// test the search string funtionality by searching for projectors in the learning components
    /// </summary>
    /// <returns>
    /// A paginated list of projectors only
    /// </returns>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_WhenGivenStringSearchProj_ShouldReturnFilteredProjectors()
    {
        // Arrange
        int learningSpaceId = 1;
        string searchString = "PROJ";
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString))
            .ReturnsAsync(_paginatedProjectorList);
        // Act
        var result = await _serviceUnderTest.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString);
        // Assert        
        result.Should().OnlyContain(x => x is Projector,
        because: "The repo returns a paginated list containing only projectors");
    
    }

    /// <summary>
    /// test the search string funtionality by searching for whiteboards in the learning components
    /// </summary>
    /// <returns>
    /// A paginated list of whiteboards only
    /// </returns>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_WhenGivenStringSearchWb_ShouldReturnFilteredWhiteboards()
    {
        // Arrange
        int learningSpaceId = 1;
        string searchString = "WB";
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString))
            .ReturnsAsync(_paginatedWhiteboardList);
        // Act
        var result = await _serviceUnderTest.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString);
        // Assert        
        result.Should().OnlyContain(x => x is Whiteboard,
        because: "The repo returns a paginated list containing only whiteboards");
    }

    /// <summary>
    /// test the search string funtionality by searching for north orientented
    /// components in the learning components
    /// </summary>
    /// <returns>
    /// A paginated list of north orienten components only
    /// </returns>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_WhenGivenStringSearchNorth_ShouldReturnFilteredNorthOrientedComponents()
    {
        // Arrange
        int learningSpaceId = 1;
        string searchString = "North";
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString))
            .ReturnsAsync(_paginatedNorthList);
        // Act
        var result = await _serviceUnderTest.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString);
        // Assert        
        result.Should().OnlyContain(x => x.Orientation.Value == "North",
        because: "The repo returns a paginated list containing only whiteboards");
    }

    /// <summary>
    /// test the search string funtionality by searching for south orientented
    /// components in the learning components
    /// </summary>
    /// <returns>
    /// A paginated list of south orienten components only
    /// </returns>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_WhenGivenStringSearchSouth_ShouldReturnFilteredSouthOrientedComponents()
    {
        // Arrange
        int learningSpaceId = 1;
        string searchString = "South";
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString))
            .ReturnsAsync(_paginatedSouthList);
        // Act
        var result = await _serviceUnderTest.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString);
        // Assert        
        result.Should().OnlyContain(x => x.Orientation.Value == "South",
        because: "The repo returns a paginated list containing only whiteboards");
    }


    /// <summary>
    /// test the search string funtionality by searching for east orientented
    /// components in the learning components
    /// </summary>
    /// <returns>
    /// A paginated list of east orienten components only
    /// </returns>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_WhenGivenStringSearchEast_ShouldReturnFilteredEastOrientedComponents()
    {
        // Arrange
        int learningSpaceId = 1;
        string searchString = "East";
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString))
            .ReturnsAsync(_paginatedEastList);
        // Act
        var result = await _serviceUnderTest.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString);
        // Assert        
        result.Should().OnlyContain(x => x.Orientation.Value == "East",
        because: "The repo returns a paginated list containing only whiteboards");
    }

    /// <summary>
    /// test the search string funtionality by searching for west orientented
    /// components in the learning components
    /// </summary>
    /// <returns>
    /// A paginated list of west orienten components only
    /// </returns>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_WhenGivenStringSearchWest_ShouldReturnFilteredWestOrientedComponents()
    {
        // Arrange
        int learningSpaceId = 1;
        string searchString = "West";
        _learningComponentRepositoryMock
            .Setup(repo => repo.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString))
            .ReturnsAsync(_paginatedWestList);
        // Act
        var result = await _serviceUnderTest.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, searchString);
        // Assert        
        result.Should().OnlyContain(x => x.Orientation.Value == "West",
        because: "The repo returns a paginated list containing only whiteboards");
    }
}
