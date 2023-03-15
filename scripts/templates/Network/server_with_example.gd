extends ServerNetwork

######################################################################
##  WARNING:
##  You can only have one network object in your project at a time.
##  This system expects a clear server app and a clear client app.
##
##  USAGE:
##  Assign this script as an autoload in your project settings, and
##  use an identicle name to the counter-part in the other app.
##
##  Example:
##      Client App: "AutoLoad/Net" -> "res://scripts/client.gd" (other script)
##      Server App: "AutoLoad/Net" -> "res://scripts/server.gd" (this script)
######################################################################

##
# User-Defined event for interacting on a peer connect signal.
#
# Params:
#         id: ENet socket connetion uuid.
##
func _peer_connected(id: int) -> void: pass

##
# User-Defined event for interacting on a peer disconnect signal.
#
# Params:
#         id: ENet socket connetion uuid.
##
func _peer_disconnected(id: int) -> void: pass

##
# Example function for utilizing this example automatically
##
func _ready() -> void:
	initialize(4321, 1)

##
# Example function for receiving a packet from the client.
#
# Format requires receive_[PacketName](params passed from client, id: int) -> bool
#
# NOTE: It is absolutely required to have an id:int param at the end reguardless of the packet or
#       it will automatically fail to handle and disconnect the client!
#
# If the function returns true, packet was handled properly.
#
# If the function returned false, there was an error and will disconnect, triggering debug logging.
##
func receive_message(message: String, id: int) -> bool:
	send_response(id, message)
	return true # successfully received

##
# Example function for sending a packet to the client.
#
# The important part of the function is the socket_send_to command.
#
# This function can be called whatever you want and even return data if you want.
#
# socket_send requires the client id, header name, then an array of the params you wish to pass with it.
##
func send_response(id: int, message: String) -> void:
	if not socket_connected(id): return
	
	socket_send_to(id, Packet.Server.response, [message])


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

# core server functions

####################################################################################################
# Initialize a new socket object and listens for connections.
#
# Params:
#         port:          The port used to connect to the server.
#         max_clients:   Limit of client connections allowed.
#         bandwidth_in:  (Optional) Limit bandwidth allowed in. 0 for none.
#         bandwidth_out: (Optional) Limit bandwidth allowed out. 0 for none.
#         ssl_pkey:      (Optional) Full path to the '.key' file used to service.
#         ssl_cert:      (Optional) Full path to the '.crt' file used to service.
#
# Returns: If the socket initialization was successful or not.
##
# func initialize(
#	port: int,
#	max_clients: int,
#	bandwidth_in: int = 0,
#	bandwidth_out: int = 0,
#	ssl_pkey: String = "",
#	ssl_cert: String = ""
#) -> bool
####################################################################################################

####################################################################################################
# Terminates the current active networking socket.
##
# terminate() -> void
####################################################################################################

####################################################################################################
# Returns the current state of the active socket connection.
#
# Params:
#         id: ENet socket connetion uuid.
##
# socket_state(id: int) -> ENetPacketPeer.PeerState
####################################################################################################

####################################################################################################
# Returns if the current active socket connection is connected or not.
#
# Params:
#         id: ENet socket connetion uuid.
##
# socket_connected(id: int) -> bool
####################################################################################################

####################################################################################################
# Ends the current active socket connection if connected.
#
# Params:
#         id: ENet socket connetion uuid.
##
# socket_disconnect(id: int) -> void
####################################################################################################

####################################################################################################
# Sends a packet to the specified client.
#
# Params:
#         id:        : The target clients ENet socket connetion uuid.
#         header     : The packet header defined in the Packet.Server enum.
#         args       : The packet data or arguments if any.
#         unreliable : Opts to allow unreliable transport. Good for speed, dangerous for syncrosity.
##
# socket_send_to(
#	id         : int,
#	header     : Packet.Server,
#	args       : Array[Variant] = [],
#	unreliable : bool = false
#) -> void
####################################################################################################

####################################################################################################
# Returns the current active socket connections ip.
#
# Params:
#         id: ENet socket connetion uuid.
##
# socket_ip(id: int) -> String
####################################################################################################
