
function readInput()
  local problemInfo = {
    rules = {},
    updates = {}
  }

  local file = io.open("inputs/day05.txt", "r")
  for line in file:lines() do
    if string.find(line, "|") then
      local a, b = line:match("(%d+)|(%d+)")
      if problemInfo.rules[tonumber(a)] == nil then
        problemInfo.rules[tonumber(a)] = {}
      end
      table.insert(problemInfo.rules[tonumber(a)], tonumber(b))
    elseif string.find(line, ",") then
      local nums = {}
      for num in line:gmatch("%d+") do
        table.insert(nums, tonumber(num))
      end
      table.insert(problemInfo.updates, nums)
    end
  end

  return problemInfo
end

function copyList(list)
  local copy = {}
  for _, v in pairs(list) do
    table.insert(copy, v)
  end
  return copy
end

function compareLists(a, b)
  if #a ~= #b then
    return false
  end

  for i = 1, #a do
    if a[i] ~= b[i] then
      return false
    end
  end

  return true
end

function getCmp(info)
  return function(a, b)
    if info.rules[a] then
      for _, val in pairs(info.rules[a]) do
        if val == b then
          return true
        end
      end
    end

    return false
  end
end

local info = readInput()  
local totalP1 = 0
local totalP2 = 0
for _, list in pairs(info.updates) do
  local copy = copyList(list)
  table.sort(copy, getCmp(info))
  local i = math.floor(#list / 2) + 1
  if compareLists(list, copy) then      
    totalP1 = totalP1 + list[i]
  else
    totalP2 = totalP2 + copy[i]
  end

end  

print("P1: " .. totalP1)
print("P2: " .. totalP2)
