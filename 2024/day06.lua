function fetchInput()
  local grid = {}
  local file = io.open("inputs/day06.txt", "r")
  local start

  for line in file:lines() do  
    local letters = {}
    for i = 1, #line do
      local val
      local chars = {
        ["."] = true,
        ["#"] = false,
        ["^"] = true
      }
      val = chars[line:sub(i,i)]
      if line:sub(i,i) == "^" then
        start = { #grid + 1, i}
      end
      table.insert(letters, val)
    end
    table.insert(grid, letters)
  end
  file:close()

  return grid, start
end

function rotateCW(dir)
  return { dir[2], -dir[1] }
end

function inBounds(grid, r, c)
  return r >= 1 and r <= #grid and c >= 1 and c <= #grid[1]
end

-- Lua is kind of brain-dead in some ways...
function tableLength(t)
  local count = 0
  for _ in pairs(t) do count = count + 1 end

  return count
end

function p1()
  local grid, start = fetchInput()
  local r, c = start[1], start[2]
  local dir = { -1, 0 }
  local steps = 0
  local visited = {}
  visited[r .. "," .. c] = true

  while true do    
    local nr = r + dir[1]
    local nc = c + dir[2]

    if not inBounds(grid, nr, nc) then
      break
    elseif grid[nr][nc] == false then
      dir = rotateCW(dir)
    else
      r, c = nr, nc      
      visited[r .. "," .. c] = true
      steps = steps + 1
    end
  end

  print("P1: " .. tableLength(visited))
end

p1()