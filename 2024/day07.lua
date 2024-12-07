function reverse(t)
  local reversed = {}
  for i = #t, 1, -1 do
    table.insert(reversed, t[i])
  end
  return reversed
end

function fetchInput()
  local formulas = {}
  local file = io.open("inputs/day07.txt", "r")
  for line in file:lines() do  
    -- 21037: 9 7 18 13
    local num, rest = line:match("^(%d+): (.*)$")
    local nums = {}
    for n in rest:gmatch("%d+") do
      table.insert(nums, tonumber(n))
    end

    local formula =
    {
      result = tonumber(num),
      operands = nums
    }
    table.insert(formulas, formula)
  end
  file:close()

  return formulas
end

function evalP1(i, total, operands, expected)
  --print("flag! " .. i .. " " .. total)
  -- base case, we are about to check the last operand
  if i == #operands then
    if total * operands[i] == expected then
      return true
    end
    if total + operands[i] == expected then
      return true
    end

    return false
  end
 
  -- recursive case, try both operations
  if evalP1(i + 1, total * operands[i], operands, expected) then
    return true
  end
  if evalP1(i + 1, total + operands[i], operands, expected) then
    return true
  end

  return false
end

function p1(formulas)
  local sum = 0
  for _, formula in ipairs(formulas) do
    if evalP1(2, formula.operands[1], formula.operands, formula.result) then
      sum = sum + formula.result
    end
  end
  print("P1: " .. sum)
end

local formulas = fetchInput()
p1(formulas)
