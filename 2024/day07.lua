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

function eval(i, total, operands, expected)
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
  if eval(i + 1, total * operands[i], operands, expected) then
    return true
  end
  if eval(i + 1, total + operands[i], operands, expected) then
    return true
  end

  return false
end

function p1(formulas)
  local sum = 0
  for _, formula in ipairs(formulas) do
    if eval(2, formula.operands[1], formula.operands, formula.result) then
      sum = sum + formula.result
    end
  end
  print("P1: " .. sum)
end

function concatAt(operands, i)
  local copy = {}
  for j = 1, i - 1 do
    table.insert(copy, operands[j])
  end
  
  local nv = tonumber(tostring(operands[i]) .. tostring(operands[i + 1]))
  table.insert(copy, nv)
  for j = i + 2, #operands do
    table.insert(copy, operands[j])
  end

  return copy
end

function concat(a, b)
  return tonumber(tostring(a) .. tostring(b))
end

function eval2(operands, expected)
  -- base case, evalulate the last two operands
  if #operands == 2 then
    if operands[1] + operands[2] == expected then
      return true
    end

    if operands[1] * operands[2] == expected then
      return true
    end

    local x = concat(operands[1], operands[2])
    if concat(operands[1], operands[2]) == expected then
      return true
    end

    return false
  end
 
  -- recursive case, try both operations
  local a = operands[1]
  local b = operands[2]
  local rest = {}
  table.move(operands, 3, #operands, 2, rest)
  rest[1] = a + b
  if eval2(rest, expected) then
    return true
  end

  rest[1] = a * b
  if eval2(rest, expected) then
    return true
  end

  rest[1] = concat(a, b)
  if eval2(rest, expected) then
    return true
  end

  return false
end

function p2(formulas)
  local sum = 0
  for _, formula in ipairs(formulas) do
    if eval2(formula.operands, formula.result) then
      sum = sum + formula.result
    end
  end
  print("P2: " .. sum)
end

local formulas = fetchInput()
p1(formulas)
p2(formulas)

