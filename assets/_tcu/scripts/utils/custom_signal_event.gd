class_name SignalData

enum SignalEventType {
    None,
    CreateMask,
    DestroyMask,
    
    #Image Popup
    CreateFloatingImage,


    GameFlow,
}

var signal_type : SignalEventType
var content : String

# Init

func _init(type_value : SignalEventType, content_value : String):
    signal_type = type_value
    content = content_value


# Static
static func CreateSignalEvent(type_param: SignalEventType, content_param : String) -> SignalData:
    return SignalData.new(type_param, content_param)

static func CreateSignal(type_param: SignalEventType, content_param: String) -> String:
    var data = CreateSignalEvent(type_param, content_param);
    return data.CreateSignalString();

static func CreateSignalFromString(input_data: String) -> SignalData:
    print("Input Data: "+ input_data)
    var parts = input_data.split("_")

    var part_type = TCU_Utils.ParseStringToEnum(SignalEventType, parts[0])
    var length = parts.size()
    var part_content : String = ""
    for i in range(1, length):
        
        part_content +=  parts[i] if i == length -1 else parts[i] + "_"
    return SignalData.new(part_type, part_content);

#Effect: Floating Image Popup
static func ParseStringImagePopupToEffectData():

    pass

# Local
func CreateSignalString() -> String:
    return "{type}_{data}".format({
        "type" :  SignalEventType.keys()[signal_type], 
        "data": content
    }
)

func ParseStringToSignal() -> String:
    return "{type}_{data}".format({
        "type" :  SignalEventType.keys()[signal_type], 
        "data": content
    }
)
