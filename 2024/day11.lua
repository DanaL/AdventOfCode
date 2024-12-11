function blink(t)
  local nt = {}
  for _, n in ipairs(t) do
    if n == "0" then
      table.insert(nt, "1")
    elseif #n % 2 == 0 then
      local len = math.floor(#n / 2)
      local a = tostring(tonumber(n:sub(1, len)))
      local b = tostring(tonumber(n:sub(-len)))
      table.insert(nt, a)
      table.insert(nt, b)
    else
      table.insert(nt, tostring(tonumber(n) * 2024))
    end
  end

  return nt
end

function fetchInput()
  local s = "30 71441 3784 580926 2 8122942 0 291"
  local t = {}
  for num in string.gmatch(s, "%S+") do
    table.insert(t, num)
  end

  return t
end

function p1()
  local input = fetchInput()
  for i = 1, 25 do
    input = blink(input)
  end

  print("P1: " .. #input)
end

p1()