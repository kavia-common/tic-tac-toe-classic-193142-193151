# Tic Tac Toe Backend (ASP.NET Core)

Simple RESTful API implementing classic Tic Tac Toe using in-memory state. No database required.

- Port: 3001
- Swagger UI: http://localhost:3001/docs
- OpenAPI JSON: http://localhost:3001/openapi.json

## Endpoints

- POST /api/games
  - Creates a new game and returns its initial state.
  - Response 201:
    ```json
    {
      "gameId": "GUID",
      "board": [["","",""],["","",""],["","",""]],
      "status": "in_progress",
      "nextPlayer": "X",
      "winner": null
    }
    ```

- GET /api/games/{id}
  - Returns current game state for the given id.

- POST /api/games/{id}/moves
  - Request body:
    ```json
    { "row": 0, "col": 2 }
    ```
  - Places the next player's mark (X then O) at the specified coordinates.
  - Returns updated game state.
  - Errors (400):
    - "Cell is already occupied."
    - "Row and Column must be between 0 and 2."
    - "Game is already over."

## Curl Examples

- Create a game:
  ```bash
  curl -X POST http://localhost:3001/api/games
  ```

- Get game status:
  ```bash
  curl http://localhost:3001/api/games/<GAME_ID>
  ```

- Make a move:
  ```bash
  curl -X POST http://localhost:3001/api/games/<GAME_ID>/moves \
    -H "Content-Type: application/json" \
    -d '{"row":0,"col":0}'
  ```

## Notes

- State is stored in-memory with a ConcurrentDictionary and will be lost when the service restarts.
- Win detection includes rows, columns, and both diagonals.
- Draw is declared when the board is full with no winner.
