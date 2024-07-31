class_name TCU_Utils


static func ParseStringToEnum(enum_type: Dictionary, string_value: String) -> int:
    var clone = enum_type.duplicate()
    var keys = clone.keys()
    var index = keys.find(string_value)
    if index != -1:
       return index
    else:
       print("String not found in enum.")
       return -1

static func ParseStringToVector2(input: String) -> Vector2:
   print("Parse String", input)
   var input_string = input.replace("(", "")
   input_string = input.replace(")", "")
   var parts = input_string.split(", ")
   var x = int(parts[0])
   var y = int(parts[1])
   var vector = Vector2(x, y)
   return vector