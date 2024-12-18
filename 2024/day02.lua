function safe(a)
  local increasing = a[#a] > a[1]

  for i = 1, #a - 1 do
    if (increasing and a[i + 1] <= a[i]) or (not increasing and a[i + 1] >= a[i]) then
      return false
    end

    local diff = math.abs(a[i + 1] - a[i])
    if diff < 1 or diff > 3 then
      return false
    end
  end

  return true
end

function copyExcept(a, i)
  local b = {}
  for j, n in ipairs(a) do
    if j ~= i then
      table.insert(b, n)
    end
  end

  return b
end

function testWithTolerance(a)
  for i = 1, #a do
    local b = copyExcept(a, i)
    if safe(b) then
      return true
    end
  end

  return false
end

function part2()
  local safeCount = 0;

  local file = io.open("inputs/day02.txt", "r")
  for line in file:lines() do
    local a = {}
    for n in line:gmatch("%d+") do
      table.insert(a, tonumber(n))      
    end

    if safe(a) then
      safeCount = safeCount + 1
    elseif testWithTolerance(a) then
      safeCount = safeCount + 1
    end
  end

  print("P2: " .. safeCount)
end

function part1()
  local safeCount = 0;

  local file = io.open("inputs/day02.txt", "r")
  for line in file:lines() do
    local a = {}
    for n in line:gmatch("%d+") do
      table.insert(a, tonumber(n))      
    end

    if safe(a) then
      safeCount = safeCount + 1
    end
  end

  print("P1: " .. safeCount)
end

part1()
part2()
