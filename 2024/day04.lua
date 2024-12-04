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

function checkLetters(a, b, c, d)
  local word = a .. b .. c .. d
  return word == "XMAS" or word == "SAMX"
end

function checkP1(p, r, c)
  local count = 0
  if c <= #p[r] - 3 and checkLetters(p[r][c], p[r][c+1], p[r][c+2], p[r][c+3]) then
    count = count + 1
  end
  
  if c <= #p[r] - 3 and r <= #p - 3 and checkLetters(p[r][c], p[r+1][c+1], p[r+2][c+2], p[r+3][c+3]) then
    count = count + 1
  end

  if r <= #p - 3 and checkLetters(p[r][c], p[r+1][c], p[r+2][c], p[r+3][c]) then
    count = count + 1
  end

  if c >= 4 and r <= #p - 3 and checkLetters(p[r][c], p[r+1][c-1], p[r+2][c-2], p[r+3][c-3]) then
    count = count + 1
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

function p2()
  local p = readPuzzle()

  local count = 0
  for r = 2, #p - 1 do
    for c = 2, #p[r] - 1 do
      local function checkAB(a, b)
        return  (a == "M" and b == "S") or (a == "S" and b == "M")
      end

      if p[r][c] == "A" then       
        local backSlash = checkAB(p[r-1][c-1], p[r+1][c+1])  
        local forSlash = checkAB(p[r+1][c-1], p[r-1][c+1])

        if forSlash and backSlash then
          count = count + 1
        end
      end
    end    
  end

  print("P2: " .. count)
end

p1()
p2()