using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Dtos.Spaces
{
    /// <summary>
    /// Unit tests for the <see cref="LearningSpaceListDto"/> record.
    /// Verifies that the constructor assigns all properties correctly and that equality operations behave as expected.
    /// </summary>
    public class LearningSpaceListDtoTests
    {
        /// <summary>
        /// Represents the identifier of the learning space.
        /// </summary>
        private int _learningSpaceId = 1;

        /// <summary>
        /// Represents the name of the learning space.
        /// </summary>
        private string _name = "Physics Lab";

        /// <summary>
        /// Represents the type of the learning space.
        /// </summary>
        private string _type = "Lab";

        /// <summary>
        /// Represents the data transfer object for a learning space list.
        /// </summary>
        private LearningSpaceListDto _dto;

        /// <summary>
        /// Represents a data transfer object for a learning space list. Copy from <see cref="_dto"/> to test equality.
        /// </summary>
        private LearningSpaceListDto _dto2;

        /// <summary>
        /// Represents a data transfer object for a learning space list. Different values from <see cref="_dto"/> to test inequality.
        /// </summary>
        private LearningSpaceListDto _dto3;

        /// <summary>
        /// Initializes test data for <see cref="LearningSpaceListDto"/> tests.
        /// </summary>
        public LearningSpaceListDtoTests()
        {
            _dto = new LearningSpaceListDto(
                _learningSpaceId,
                _name,
                _type
            );

            _dto2 = new LearningSpaceListDto(
                _learningSpaceId,
                _name,
                _type
            );

            _dto3 = new LearningSpaceListDto(
                2,
                "Chemistry Lab",
                "Classroom"
            );
        }

        /// <summary>
        /// Verifies that the LearningSpaceId property is assigned correctly by the constructor.
        /// </summary>
        [Fact]
        public void Constructor_WithValidArguments_AssignsLearningSpaceIdCorrectly()
        {
            var dto = new LearningSpaceListDto(
                _learningSpaceId,
                _name,
                _type
            );

            dto.LearningSpaceId.Should().Be(_learningSpaceId, because: "constructor should assign LearningSpaceId");
        }

        /// <summary>
        /// Verifies that the Name property is assigned correctly by the constructor.
        /// </summary>
        [Fact]
        public void Constructor_WithValidArguments_AssignsNameCorrectly()
        {
            var dto = new LearningSpaceListDto(
                _learningSpaceId,
                _name,
                _type
            );

            dto.Name.Should().Be(_name, because: "constructor should assign Name");
        }

        /// <summary>
        /// Verifies that the Type property is assigned correctly by the constructor.
        /// </summary>
        [Fact]
        public void Constructor_WithValidArguments_AssignsTypeCorrectly()
        {
            var dto = new LearningSpaceListDto(
                _learningSpaceId,
                _name,
                _type
            );

            dto.Type.Should().Be(_type, because: "constructor should assign Type");
        }

        /// <summary>
        /// Verifies that two <see cref="LearningSpaceListDto"/> instances with the same property values are considered equal.
        /// </summary>
        [Fact]
        public void Equality_WithSameValues_ReturnsTrue()
        {
            _dto.Should().Be(_dto2, because: "records with same values should be equal");
            (_dto == _dto2).Should().BeTrue(because: "record equality operator should return true for same values");
        }

        /// <summary>
        /// Verifies that two <see cref="LearningSpaceListDto"/> instances with different property values are not considered equal.
        /// </summary>
        [Fact]
        public void Equality_WithDifferentValues_ReturnsFalse()
        {
            _dto.Should().NotBe(_dto3, because: "records with different values should not be equal");
            (_dto != _dto3).Should().BeTrue(because: "record inequality operator should return true for different values");
        }
    }
}
