function inBounds(r, c, rows, cols)
  return r >= 1 and r <= rows and c >= 1 and c <= cols
end

function fetchInput()
  local file = io.open("inputs/day08.txt", "r")
  local row = 1
  local cols = 0
  local antennae = {}
  for line in file:lines() do    
    cols = #line
    for i = 1, #line do
      local ch = line:sub(i, i)
      if ch ~= "." then
        if not antennae[ch] then
          antennae[ch] = {}
        end
        table.insert(antennae[ch], { row, i })
      end
    end

    row = row + 1
  end
  file:close()

  return { antennae, row - 1, cols }
end

function checkForAntinodes(antennae, antinodes, key, rows, cols)
  local pos = antennae[key]
  for i = 1, #pos do
    for j = i + 1, #pos do
      local dr = (pos[i][1] - pos[j][1]) * 2
      local dc = (pos[i][2] - pos[j][2]) * 2
      
      if inBounds(pos[i][1] - dr, pos[i][2] - dc, rows, cols) then        
        local anKey = tostring(pos[i][1] - dr) .. "," .. tostring(pos[i][2] - dc)
        antinodes[anKey] = true
      end
      
      if inBounds(pos[j][1] + dr, pos[j][2] + dc, rows, cols) then
        local anKey = tostring(pos[j][1] + dr) .. "," .. tostring(pos[j][2] + dc)
        antinodes[anKey] = true
      end
    end
  end
end

function p1()
  local antennae, rows, cols = table.unpack(fetchInput())
  local antinodes = {}

  for key, _ in pairs(antennae) do
    checkForAntinodes(antennae, antinodes, key, rows, cols)    
  end

  -- Can't believe lua doesn't have a proper hash table or a built-in way to 
  -- count the number of entries in a hashtable...
  local count = 0
  for _, _ in pairs(antinodes) do
    count = count + 1
  end

  print("P1: " .. count)
end

p1()
