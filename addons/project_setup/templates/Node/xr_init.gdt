extends Node 

var interface : XRInterface = XRServer.find_interface("OpenXR")

func _ready():
	if interface and interface.is_initialized():
		print_debug("OpenXR initialised successfully")
		get_viewport().use_xr = true
	else:
		print_debug("OpenXR not initialised, please check if your headset is connected")
