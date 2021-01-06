using DeveloperPath.Application.Common.Exceptions;
using DeveloperPath.Application.TodoItems.Commands.CreateTodoItem;
using DeveloperPath.Application.TodoItems.Commands.DeleteTodoItem;
using DeveloperPath.Application.TodoLists.Commands.CreateTodoList;
using DeveloperPath.Domain.Entities;
using FluentAssertions;
using System.Threading.Tasks;
using NUnit.Framework;
using System;

namespace DeveloperPath.Application.IntegrationTests.TodoItems.Commands
{
    using static Testing;
    [Obsolete("Left from project template. Remove")]
    public class DeleteTodoItemTests : TestBase
    {
        [Test]
        public void ShouldRequireValidTodoItemId()
        {
            var command = new DeleteTodoItemCommand { Id = 99 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteTodoItem()
        {
            var listId = await SendAsync(new CreateTodoListCommand
            {
                Title = "New List"
            });

            var itemId = await SendAsync(new CreateTodoItemCommand
            {
                ListId = listId,
                Title = "New Item"
            });

            await SendAsync(new DeleteTodoItemCommand
            {
                Id = itemId
            });

            var list = await FindAsync<TodoItem>(listId);

            list.Should().BeNull();
        }
    }
}
