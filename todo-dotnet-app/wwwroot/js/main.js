document.addEventListener("DOMContentLoaded", () => {
  const todoListContainer = document.getElementById("todo-list");
  const todoInput = document.getElementById("todo-input");
  const addTodoBtn = document.getElementById("add-todo-btn");

  // Fetch todos from the API and render them
  const fetchTodos = async () => {
    const response = await fetch("/api/Todo");
    const todos = await response.json();

    // Sort todos by 'Timestamp' (from oldest to newest)
    todos.sort((a, b) => new Date(a.timestamp) - new Date(b.timestamp));

    todoListContainer.innerHTML = ""; // Clear existing todos
    todos.forEach((todo) => {
      const todoItem = createTodoElement(todo);
      todoListContainer.appendChild(todoItem);
    });
  };

  // Create a new todo element
  const createTodoElement = (todo) => {
    const div = document.createElement("div");
    div.classList.add("todo-item");
    if (todo.isComplete) {
      div.classList.add("complete");
    }

    div.innerHTML = `
            <span>${todo.name}</span>
            <div class="todo-actions">
                <button onclick="toggleComplete('${todo.rowKey}')">✓</button>
                <button onclick="deleteTodo('${todo.rowKey}')">❌</button>
            </div>
        `;
    return div;
  };

  // Add new todo
  const addTodo = async () => {
    const newTodoName = todoInput.value.trim();
    if (newTodoName === "") return;

    const newTodo = { name: newTodoName, isComplete: false };
    await fetch("/api/Todo", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(newTodo),
    });

    todoInput.value = ""; // Clear the input
    fetchTodos(); // Refresh the todo list
  };

  // Trigger the addTodo function when the Add Todo button is clicked
  addTodoBtn.addEventListener("click", addTodo);

  // Trigger the addTodo function when Enter key is pressed in the input field
  todoInput.addEventListener("keydown", (event) => {
    if (event.key === "Enter") {
      addTodo(); // Call the addTodo function
    }
  });

  // Toggle completion (strike-through)
  window.toggleComplete = async (rowKey) => {
    const response = await fetch(`/api/Todo/${rowKey}`);
    const todo = await response.json();

    todo.isComplete = !todo.isComplete;
    await fetch(`/api/Todo/${rowKey}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(todo),
    });

    fetchTodos(); // Refresh the todo list
  };

  // Delete a todo item
  window.deleteTodo = async (rowKey) => {
    await fetch(`/api/Todo/${rowKey}`, { method: "DELETE" });
    fetchTodos(); // Refresh the todo list
  };

  // Initial fetch of todos
  fetchTodos();
});
