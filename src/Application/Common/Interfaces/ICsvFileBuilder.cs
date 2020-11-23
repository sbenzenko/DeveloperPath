using DeveloperPath.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace DeveloperPath.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
