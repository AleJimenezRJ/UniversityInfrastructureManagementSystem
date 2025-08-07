using FluentAssertions;
using System;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.ComponentsManagement
{
    /// <summary>
    /// Contains unit tests for the global mapper <see cref="GlobalMapper"/>,
    /// verifying bidirectional mapping between domain entities and their DTOs.
    /// </summary>
    public class GlobalMapperTests
    {
        /// <summary>
        /// Mapper instance to be tested.
        /// </summary>
        private readonly GlobalMapper _mapper;

        /// <summary>
        /// Reusable <see cref="Projector"/> instance used in tests.
        /// </summary>
        private readonly Projector _testProjector;

        /// <summary>
        /// Initializes a new instance of <see cref="GlobalMapperTests"/>,
        /// sets up the mapper and creates a test projector entity.
        /// </summary>
        public GlobalMapperTests()
        {
            _mapper = new GlobalMapper();

            _testProjector = CreateTestProjector();
        }

        /// <summary>
        /// Creates a <see cref="Projector"/> instance with default or custom values for testing.
        /// </summary>
        /// <param name="content">Projected content.</param>
        /// <param name="id">Component identifier.</param>
        /// <param name="orientationValue">Orientation of the projector.</param>
        /// <param name="posX">X coordinate of the position.</param>
        /// <param name="posY">Y coordinate of the position.</param>
        /// <param name="posZ">Z coordinate of the position.</param>
        /// <param name="areaWidth">Width of the projection area.</param>
        /// <param name="areaHeight">Height of the projection area.</param>
        /// <param name="dimWidth">Width of the projector dimensions.</param>
        /// <param name="dimHeight">Height of the projector dimensions.</param>
        /// <param name="dimDepth">Depth of the projector dimensions.</param>
        /// <returns>A configured <see cref="Projector"/> instance.</returns>
        private Projector CreateTestProjector(
            string content = "Content",
            int id = 1,
            string orientationValue = "North",
            double posX = 2.5, double posY = 3, double posZ = 4,
            int areaWidth = 5, int areaHeight = 5,
            int dimWidth = 15, int dimHeight = 20, int dimDepth = 10
        )
        {
            return new Projector(
                content,
                Area2D.Create(areaWidth, areaHeight),
                id,
                Orientation.Create(orientationValue),
                Coordinates.Create(posX, posY, posZ),
                Dimension.Create(dimWidth, dimHeight, dimDepth)
            );
        }

        /// <summary>
        /// Tests that mapping a registered <see cref="Projector"/> entity to DTO returns the correct DTO.
        /// </summary>
        [Fact]
        public void ToDto_ShouldMapProjectorToDto_WhenRegistered()
        {
            var dto = _mapper.ToDto(_testProjector);

            dto.Should().NotBeNull();
            dto.Should().BeOfType<ProjectorDto>();
            dto.Id.Should().Be(_testProjector.ComponentId);
            ((ProjectorDto)dto).ProjectedContent.Should().Be(_testProjector.ProjectedContent);
        }

        /// <summary>
        /// Tests that mapping an unknown component type throws <see cref="NotSupportedException"/>.
        /// </summary>
        [Fact]
        public void ToDto_ShouldThrowNotSupportedException_WhenTypeNotRegistered()
        {
            var unknownComponent = new UnknownLearningComponent();

            Action act = () => _mapper.ToDto(unknownComponent);

            act.Should().Throw<NotSupportedException>()
                .WithMessage($"No ID-based mapper registered for type {unknownComponent.GetType().Name}.");
        }

        /// <summary>
        /// Tests that mapping a DTO without ID to entity returns a properly mapped entity.
        /// </summary>
        [Fact]
        public void ToEntity_ShouldMapDtoWithoutIdToEntity_WhenRegistered()
        {
            var dtoNoId = new ProjectorNoIdDto(
                Orientation: "North",
                Position: new PositionDto(1, 2, 3),
                Dimensions: new DimensionsDto(10, 20, 30),
                ProjectionArea: new ProjectionAreaDto(5, 5),
                ProjectedContent: "Content"
            );

            var entity = _mapper.ToEntity(dtoNoId);

            entity.Should().NotBeNull();
            entity.Should().BeOfType<Projector>();
            var projector = (Projector)entity;
            projector.ProjectedContent.Should().Be("Content");
            projector.Orientation.Value.Should().Be("North");
        }

        /// <summary>
        /// Tests that mapping an unknown DTO type throws <see cref="NotSupportedException"/>.
        /// </summary>
        [Fact]
        public void ToEntity_ShouldThrowNotSupportedException_WhenDtoNotRegistered()
        {
            var unknownDto = new UnknownLearningComponentNoIdDto(
                "North",
                new PositionDto(0, 0, 0),
                new DimensionsDto(1, 1, 1)
            );

            Action act = () => _mapper.ToEntity(unknownDto);

            act.Should().Throw<NotSupportedException>()
                .WithMessage($"No No-ID mapper registered for DTO type {unknownDto.GetType().Name}.");
        }

        /// <summary>
        /// Dummy class inheriting <see cref="Projector"/> to test unregistered type scenarios.
        /// </summary>
        private class UnknownLearningComponentFail : Projector
        {
            public UnknownLearningComponentFail() : base(
                "DummyContent",
                Area2D.Create(1, 1),
                999,
                Orientation.Create("North"),
                Coordinates.Create(0, 0, 0),
                Dimension.Create(1, 1, 1))
            { }
        }

        /// <summary>
        /// Dummy DTO record for unregistered DTO type tests.
        /// </summary>
        private record UnknownLearningComponentNoIdDtoFail(
            string Orientation,
            PositionDto Position,
            DimensionsDto Dimensions
        ) : LearningComponentNoIdDto(Orientation, Position, Dimensions);

        /// <summary>
        /// Dummy component class for unregistered type tests.
        /// </summary>
        private class UnknownLearningComponent : LearningComponent { }

        /// <summary>
        /// Dummy DTO record for unregistered DTO type tests.
        /// </summary>
        private record UnknownLearningComponentNoIdDto(
            string Orientation,
            PositionDto Position,
            DimensionsDto Dimensions
        ) : LearningComponentNoIdDto(Orientation, Position, Dimensions);
    }
}