extends HybridNetwork

######################################################################
##  WARNING:
##  You can only have one network object in your project at a time.
##
##  USAGE:
##  Assign this script as an autoload in your project settings.
##
##  Example:
##      App: "AutoLoad/Net" -> "res://scripts/network.gd"
######################################################################

##
# User-Defined event for interacting on a successful joiner-side connection attempt.
##
func _connection_succeeded() -> void: pass

##
# User-Defined event for interacting on a failed joiner-side connection attempt.
##
func _connection_failed() -> void: pass

##
# User-Defined event for interacting on a joiner-side connection termination.
##
func _connection_ended() -> void: pass

##
# User-Defined event for interacting on a host-side peer connect signal.
#
# Params:
#         id: ENet socket connetion uuid.
##
func _peer_connected(id: int) -> void: pass

##
# User-Defined event for interacting on a host-side peer disconnect signal.
#
# Params:
#         id: ENet socket connetion uuid.
##
func _peer_disconnected(id: int) -> void: pass

##
# Connect or reconnect to the host.
##
func connect_to_host() -> void:
	terminate()
	join("127.0.0.1", 4321)

##
# Example function for sending a packet to the host.
#
# The important part of the function is the socket_send command.
#
# This function can be called whatever you want and even return data if you want.
#
# socket_send requires the header name, then an array of the params you wish to pass with it.
##
func send_message(message: String) -> void:
	if not socket_connected(): return
	
	socket_send_to(1, Packet.Hybrid.message, [message])

##
# Example function for receiving a packet from the sender.
#
# Format requires receive_[PacketName](params passed from sender, id: int) -> bool
#
# NOTE: It is absolutely required to have an id:int param at the end reguardless of the packet or
#       it will automatically fail to handle and disconnect the socket!
#
# If the function returns true, packet was handled properly.
#
# If the function returned false, there was an error and will disconnect, triggering debug logging.
##
func receive_message(message: String, id: int) -> bool:
	send_response(id, message)
	return true # successfully received

##
# Example function for sending a packet to the connection.
#
# The important part of the function is the socket_send_to command.
#
# This function can be called whatever you want and even return data if you want.
#
# socket_send requires the connection id, header name, then an array of the params you wish to pass with it.
##
func send_response(id: int, message: String) -> void:
	if not socket_connected(id): return
	
	# display it for ourselves too before relay
	if not (receive_response(message, id)): return

	socket_send_to(id, Packet.Hybrid.response, [message])

##
# Example function for receiving a packet from the host.
#
# Format requires receive_[PacketName](params passed from sender, id:int) -> bool
#
# NOTE: It is absolutely required to have an id:int param at the end reguardless of the packet or
#       it will automatically fail to handle and disconnect the socket!
#
# If the function returns true, packet was handled properly.
#
# If the function returned false, there was an error and will disconnect, triggering debug logging.
##
func receive_response(message: String, id: int) -> bool:
	print(message)
	return true # successfully received


####################################
##                                ##
##  Available Internal Functions  ##
##                                ##
####################################

# core network functions

####################################################################################################
# Provides access to log debug information in both backend and frontend.
##
# log_debug(message: String) -> void
####################################################################################################

####################################################################################################
# Provides access to log information in both backend and frontend.
##
# log_status(message: String) -> void
####################################################################################################

####################################################################################################
# Gets the current active networking socket. Generally used for manipulation of the active socket connections.
#
# Returns: A Node that looks after P2P multiplayer networking.
##
# get_socket() -> ENetMultiplayerPeer
####################################################################################################

# core hybrid functions

####################################################################################################
# Returns if the active socket is a session host.
##
# is_host() -> bool
####################################################################################################

####################################################################################################
# Initialize a new socket object and listens for connections.
#
# Params:
#         port:          The port used to connect to the host.
#         max_clients:   Limit of client connections allowed.
#         bandwidth_in:  (Optional) Limit bandwidth allowed in. 0 for none.
#         bandwidth_out: (Optional) Limit bandwidth allowed out. 0 for none.
#         ssl_pkey:      (Optional) Full path to the '.key' file used to service.
#         ssl_cert:      (Optional) Full path to the '.crt' file used to service.
#
# Returns: If the socket initialization was successful or not.
##
# host(
#	port: int,
#	max_clients: int,
#	bandwidth_in: int = 0,
#	bandwidth_out: int = 0,
#	ssl_pkey: String = "",
#	ssl_cert: String = ""
#) -> bool
####################################################################################################

####################################################################################################
# Initialize a new socket object and attempts a connection.
#
# Params:
#         ip:            The ip the host is located on.
#         port:          The port used to connect to the host.
#         bandwidth_in:  (Optional) Limit bandwidth allowed in. 0 for none.
#         bandwidth_out: (Optional) Limit bandwidth allowed out. 0 for none.
#         ssl_cert:      (Optional) Full path to the '.crt' file used to connect.
#         ssl_host:      (Optional) Host name used to verify crt.
#         ssl_verify:    (Optional) 3rd party extra integrity check.
#
# Returns: If the socket initialization was successful or not.
##
# join(
#	ip: String,
#	port: int,
#	bandwidth_in: int = 0,
#	bandwidth_out: int = 0,
#	ssl_cert: String = "",
#	ssl_host: String = "",
#	ssl_verify: bool = false
#) -> bool
####################################################################################################

####################################################################################################
# Terminates the current active networking socket.
##
# terminate() -> void
####################################################################################################

####################################################################################################
# Receives a packet by header, and passes any args to the handler for the packet. If this packet is invalid, it will be refused.
#
# Params:
#         id:        : The target hybrid ENet socket connetion uuid.
#         header     : The packet header defined in the Packet.Hybrid enum.
#         args       : The packet data or arguments if any.
##
# _receive_data(id: int, header: Packet.Hybrid, args: Array[Variant]) -> void
####################################################################################################
 
####################################################################################################
# Returns the current state of the active socket connection.
#
# Params:
#         id: ENet socket connetion uuid. (1 for host)
##
# socket_state(id: int = 1) -> int
####################################################################################################

####################################################################################################
# Returns if the current active socket connection is connected or not.
#
# Params:
#         id: ENet socket connetion uuid. (1 for host)
##
# socket_connected(id: int = 1) -> bool
####################################################################################################

####################################################################################################
# Ends the current active socket connection if connected.
#
# Params:
#         id: ENet socket connetion uuid. (1 for host)
##
# socket_disconnect(id: int = 1) -> void
####################################################################################################

####################################################################################################
# Sends a packet to the specified connection.
#
# Params:
#         id:        : The target ENet socket connetion uuid. (1 for host)
#         header     : The packet header defined in the Packet.Hybrid enum.
#         args       : The packet data or arguments if any.
#         unreliable : Opts to allow unreliable transport. Good for speed, dangerous for syncrosity.
#         channel    : The channel number to send on.
##
# socket_send_to(
#	id         : int,
#	header     : Packet.Hybrid,
#	args       : Array[Variant] = [],
#	unreliable : bool = false,
#	channel    : int = 1
#) -> void
####################################################################################################

####################################################################################################
# Returns the current active socket connections ip.
#
# Params:
#         id: ENet socket connetion uuid. (1 for host)
##
# socket_ip(id: int = 1) -> String
####################################################################################################