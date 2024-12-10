function inBounds(r, c, height, width)
  return r >= 0 and r < height and c >= 0 and c < width
end

function toIdx(r, c, width)
  return r * width + c + 1
end

function fromIdx(idx, width)
  idx = idx - 1
  local r = math.floor(idx / width)
  local c = idx % width
  return r, c
end

function fetchInput()
  local height = 0
  local width = 0
  local s = ""
  local f = io.open("inputs/day10.txt", "r")
  for line in f:lines() do
    height = height + 1
    width = #line
    s = s .. line
  end

  return { s, height, width }
end

function countPaths(i, map, height, width)
  local adj = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } }
  local r, c = fromIdx(i, width)
  local q = {}
  local visited = {}
  local peaks = {}
  table.insert(q, { r, c, 0 })

  while #q > 0 do
    local r, c, d = table.unpack(table.remove(q, 1))

    if visited[tostring(r) .. "," .. tostring(c)] then
      goto continue2
    end

    visited[tostring(r) .. "," .. tostring(c)] = true
    
    for _, a in ipairs(adj) do
      local nr, nc = r + a[1], c + a[2]
      if not inBounds(nr, nc, height, width) then
        goto continue
      end
      
      local j = toIdx(nr, nc, width)
      local n = tonumber(map:sub(j, j))
  
      if d == 8 and n == 9 then
        peaks[tostring(nr) .. "," .. tostring(nc)] = true        
      elseif n == d + 1 and not visited[tostring(nr) .. "," .. tostring(nc)] then
        table.insert(q, { nr, nc, n })
      end

      ::continue::
    end

    ::continue2::
  end


  local paths = 0
  for _ in pairs(peaks) do
    paths = paths + 1
  end
  return paths
end

function p1()
  local map, height, width = table.unpack(fetchInput())
  local paths = 0

  for i = 1, #map do
    local n = map:sub(i,i)
    if n == "0" then
      paths = paths + countPaths(i, map, height, width)
    end
  end

  print("P1: ", paths)
end

p1()

