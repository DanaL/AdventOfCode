function part2()
  local a = {}
  local b = {}

  local file = io.open("inputs/day01.txt", "r")
  for line in file:lines() do
    local int1, int2 = line:match("(%d+)%s+(%d+)")
    
    table.insert(a, tonumber(int1))
    local n = tonumber(int2)
    b[n] = (b[n] or 0) + 1
  end
  file:close()

  local similarity = 0
  for _, n in pairs(a) do
    if b[n] then
      similarity = similarity + n * b[n]
    end
  end
    
  print("P2: " .. similarity)
end

function part1()
  local a = {}
  local b = {}

  local file = io.open("inputs/day01.txt", "r")
  for line in file:lines() do
    local int1, int2 = line:match("(%d+)%s+(%d+)")
    
    table.insert(a, tonumber(int1))
    table.insert(b, tonumber(int2))
  end
  file:close()

  table.sort(a)
  table.sort(b)

  local totalDistance = 0
  for i = 1, #a do
    totalDistance = totalDistance + math.abs(a[i] - b[i])
  end

  print("P1: " .. totalDistance)
end

part1()
part2()