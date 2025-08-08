using Ambev.UsersDeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.UsersDeveloperEvaluation.Domain.Repositories;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using NSubstitute;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Application.Users
{
    public class DeleteUserHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly DeleteUserHandler _handler;
        private readonly IValidator<DeleteUserCommand> _validator; // Adicionar o validador mock

        public DeleteUserHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _handler = new DeleteUserHandler(_userRepository);
            _validator = Substitute.For<IValidator<DeleteUserCommand>>();
        }

        [Fact(DisplayName = "Given valid user id When deleting user Then should delete user successfully")]
        public async Task Handle_ValidId_DeletesUserSuccessfully()
        {
            // Arrange
            var command = new DeleteUserCommand(Guid.NewGuid());
            _userRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(true));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _userRepository.Received(1).DeleteAsync(command.Id, CancellationToken.None);
        }

        [Fact(DisplayName = "Given invalid command When deleting user Then should throw ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new DeleteUserCommand(Guid.Empty); // Comando inválido (ex: ID vazio)
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Id", "Id must not be empty.")
            };
            var validationResult = new ValidationResult(validationFailures);

            _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(validationResult));

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact(DisplayName = "Given invalid user id When deleting user Then should throw NotFoundException")]
        public async Task Handle_InvalidId_ThrowsNotFoundException()
        {
            // Arrange
            var command = new DeleteUserCommand(Guid.NewGuid());
            _userRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(false));

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                    .WithMessage($"User with ID {command.Id} not found");
        }
    }
}