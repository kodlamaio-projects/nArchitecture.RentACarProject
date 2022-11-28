﻿using Application.Features.Colors.Commands.DeleteColor;
using Application.Tests.Mocks.FakeData;
using Application.Tests.Mocks.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.Features.Colors.Commands.DeleteColor.DeleteColorCommand;

namespace Application.Tests.Features.Colors.Commands.DeleteColor
{
    public class DeleteColorTests : ColorMockRepository
    {
        private readonly DeleteColorCommandHandler _handler;
        private readonly DeleteColorCommand _command;
        public DeleteColorTests(ColorFakeData fakeData, DeleteColorCommand command) : base(fakeData)
        {
            _command = command;
            _handler = new(MockRepository.Object, Mapper, BusinessRules);
        }

        [Fact]
        public async Task DeleteColorSuccessfully()
        {
            _command.Id = 1;
            var result = await _handler.Handle(_command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ColorIdNotExistsShouldReturnError()
        {
            _command.Id = 6;
            await Assert.ThrowsAsync<BusinessException>(async () => await _handler.Handle(_command, CancellationToken.None));
        }
    }
}