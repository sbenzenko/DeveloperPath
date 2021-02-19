using DeveloperPath.Application.TodoLists.Queries.ExportTodos;
using System;
using System.Collections.Generic;

namespace DeveloperPath.Application.Common.Interfaces
{
    [Obsolete("This interface is from the project template. Should not be used")]
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
