Token = {
  WHITESPACE = 1,
  NUMBER = 2,
  OPERATOR = 3,
  LETTER = 4,
  LEFT_PAREN = 5,
  RIGHT_PAREN = 6,
  COMMA = 7,
  MISC = 8
}

function atEnd(txtInfo)
  return txtInfo.pos > #txtInfo.txt
end

function peek(txtInfo)
  return txtInfo.txt:sub(txtInfo.pos, txtInfo.pos)
end

function advance(txtInfo)
  local ch = txtInfo.txt:sub(txtInfo.pos, txtInfo.pos)
  txtInfo.pos = txtInfo.pos + 1

  return ch
end

function matchesText(txtInfo, keyword)
  if txtInfo.pos + #keyword > #txtInfo.txt then
    return false
  end

  local i = txtInfo.pos
  for j = 1, #keyword do
    local a = txtInfo.txt:sub(i + j - 1, i + j - 1)
    local b = keyword:sub(j, j)
    
    if a ~= b then
      return false
    end
  end

  return true
end

function scanString(txtInfo, ch)
  local str = ch

  while peek(txtInfo):match("%a") ~= nil do
    str = str .. advance(txtInfo)
  end

  return str
end

function scanNumber(txtInfo, ch)
  local num = ch

  while peek(txtInfo):match("%d") ~= nil do
    num = num .. advance(txtInfo)
  end

  return num
end

function scan(txtInfo)
  local tokens = {}

  while not atEnd(txtInfo) do
    local ch = advance(txtInfo)

    if ch:match("%s") ~= nil then
      table.insert(tokens, {
        type = Token.WHITESPACE,
        value = ch
      })
    elseif ch:match("%d") ~= nil then
      table.insert(tokens, {
        type = Token.NUMBER,
        value = scanNumber(txtInfo, ch)
      })
    elseif ch == "(" then
      table.insert(tokens, {
        type = Token.LEFT_PAREN,
        value = ch
      })
    elseif ch == ")" then
      table.insert(tokens, {
        type = Token.RIGHT_PAREN,
        value = ch
      })
    elseif ch == "," then
      table.insert(tokens, {
        type = Token.COMMA,
        value = ch
      })
    elseif ch == "m" and matchesText(txtInfo, "ul") then
      table.insert(tokens, {
        type = Token.OPERATOR,
        value = "mul"
      })
      advance(txtInfo)
      advance(txtInfo)
    elseif ch:match("%a") ~= nil then
      table.insert(tokens, {
        type = Token.LETTER,
        value = ch
      })
    else
      table.insert(tokens, {
        type = Token.MISC,
        value = ch
      })
    end
  end

  return tokens
end

function part1()
  local file = io.open("inputs/day03.txt", "r")
  local txt = file:read("*all")
  file:close()

  local txtInfo = {
    txt = txt,
    pos = 1
  };

  tokens = scan(txtInfo) 

  local tokenTypeLabels = {
    [Token.LEFT_PAREN] = "LEFT_PAREN",
    [Token.RIGHT_PAREN] = "RIGHT_PAREN", 
    [Token.COMMA] = "COMMA",
    [Token.OPERATOR] = "OPERATOR",
    [Token.LETTER] = "LETTER",
    [Token.MISC] = "MISC",
    [Token.WHITESPACE] = "WHITESPACE",
    [Token.NUMBER] = "NUMBER"
  }

  for _, token in ipairs(tokens) do
    local type = tokenTypeLabels[token.type] or tostring(token.type)
    print(type .. " " .. token.value)
    end
end

part1()