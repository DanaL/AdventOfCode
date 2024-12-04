function readPuzzle()
  local puzzle = {}
  local file = io.open("inputs/day04.txt", "r")
  for line in file:lines() do
    local letters = {}
    for i = 1, #line do
      table.insert(letters, line:sub(i,i))
    end
    table.insert(puzzle, letters)
  end
  file:close()

  return puzzle
end

function checkP1(puzzle, r, c)
  count = 0
  if c <= #puzzle[r] - 3 then
    local word = puzzle[r][c] .. puzzle[r][c+1] .. puzzle[r][c+2] .. puzzle[r][c+3]
    if word == "XMAS" or word == "SAMX" then
      count = count + 1
    end
  end
  
  if c <= #puzzle[r] - 3 and r <= #puzzle - 3 then
    local word = puzzle[r][c] .. puzzle[r+1][c+1] .. puzzle[r+2][c+2] .. puzzle[r+3][c+3]
    if word == "XMAS" or word == "SAMX" then
      count = count + 1
    end
  end

  if r <= #puzzle - 3 then
    local word = puzzle[r][c] .. puzzle[r+1][c] .. puzzle[r+2][c] .. puzzle[r+3][c]
    if word == "XMAS" or word == "SAMX" then
      count = count + 1
    end
  end

  if c >= 4 and r <= #puzzle - 3 then
    local word = puzzle[r][c] .. puzzle[r+1][c-1] .. puzzle[r+2][c-2] .. puzzle[r+3][c-3]
    if word == "XMAS" or word == "SAMX" then
      count = count + 1
    end
  end

  return count
end

function p1()
  local puzzle = readPuzzle()

  local count = 0
  for r = 1, #puzzle do
    for c = 1, #puzzle[r] do
      count = count + checkP1(puzzle, r, c)
    end
  end

  print("P1: " .. count)
end

p1()