extends ClientNetwork

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
##      Client App: "AutoLoad/Net" -> "res://scripts/client.gd" (this script)
##      Server App: "AutoLoad/Net" -> "res://scripts/server.gd" (other script)
######################################################################

##
# User-Defined event for interacting on a successful connection attempt.
##
func _connection_succeeded() -> void: pass

##
# User-Defined event for interacting on a failed connection attempt.
##
func _connection_failed() -> void: pass

##
# User-Defined event for interacting on a connection termination.
##
func _connection_ended() -> void: pass

##
# Connect or reconnect to the server.
##
func connect_to_server(ip: String, port: int) -> void:
	terminate()
	initialize(ip, port)

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

# core client functions

####################################################################################################
# Initialize a new socket object and attempts a connection.
#
# Params:
#         ip:            The ip the server is located on.
#         port:          The port used to connect to the server.
#         bandwidth_in:  (Optional) Limit bandwidth allowed in. 0 for none.
#         bandwidth_out: (Optional) Limit bandwidth allowed out. 0 for none.
#         ssl_cert:      (Optional) Full path to the '.crt' file used to connect.
#         ssl_host:      (Optional) Host name used to verify crt.
#         ssl_verify:    (Optional) 3rd party extra integrity check.
#
# Returns: If the socket initialization was successful or not.
##
# initialize(
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
# Returns the current state of the active socket connection.
##
# socket_state() -> ENetPacketPeer.PeerState
####################################################################################################

####################################################################################################
# Returns if the current active socket connection is connected or not.
##
# socket_connected() -> bool
####################################################################################################

####################################################################################################
# Ends the current active socket connection if connected.
##
# socket_disconnect() -> void
####################################################################################################

####################################################################################################
# Sends a packet to the server. Assumes connection is active when invoked, and will invoke _connection_ended if connection is not detected.
#
# Params:
#         header     : The packet header defined in the Packet.Client enum.
#         args       : The packet data or arguments if any.
#         unreliable : Opts to allow unreliable transport. Good for speed, dangerous for syncrosity.
#         channel    : The channel number to send on.
##
# socket_send(
#	header     : Packet.Client,
#	args       : Array[Variant] = [],
#	unreliable : bool = false,
#	channel    : int = 1
#) -> void
####################################################################################################
