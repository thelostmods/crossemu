class_name Network extends Node

################################################################################
#                        Copyright 2022-2023 ArchaicSoft                       #
#                                                                              #
# Boost Software License - Version 1.0 - August 17th, 2003                     #
#                                                                              #
# Permission is hereby granted, free of charge, to any person or organization  #
# obtaining a copy of the software and accompanying documentation covered by   #
# this license (the "Software") to use, reproduce, display, distribute,        #
# execute, and transmit the Software, and to prepare derivative works of the   #
# Software, and to permit third-parties to whom the Software is furnished to   #
# do so, all subject to the following:                                         #
#                                                                              #
# The copyright notices in the Software and this entire statement, including   #
# the above license grant, this restriction and the following disclaimer,      #
# must be included in all copies of the Software, in whole or in part, and     #
# all derivative works of the Software, unless such copies or derivative       #
# works are solely in the form of machine-executable object code generated by  #
# a source language processor.                                                 #
#                                                                              #
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR   #
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,     #
# FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT    #
# SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE    #
# FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,  #
# ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER  #
# DEALINGS IN THE SOFTWARE.                                                    #
################################################################################

# remote accessors

@rpc("authority", "reliable", "call_remote", 0)
func client_receive_reliable(_header: Packet.Server, _args: Array[Variant]) -> void: pass

@rpc("authority", "unreliable", "call_remote", 1)
func client_receive_unreliable(_header: Packet.Server, _args: Array[Variant]) -> void: pass

@rpc("any_peer", "reliable", "call_remote", 0)
func server_receive_reliable_channel_1(header: Packet.Client, args: Array[Variant]) -> void: pass

@rpc("any_peer", "reliable", "call_remote", 1)
func server_receive_reliable_channel_2(header: Packet.Client, args: Array[Variant]) -> void: pass

@rpc("any_peer", "unreliable", "call_remote", 2)
func server_receive_unreliable_channel_1(header: Packet.Client, args: Array[Variant]) -> void: pass

@rpc("any_peer", "unreliable", "call_remote", 3)
func server_receive_unreliable_channel_2(header: Packet.Client, args: Array[Variant]) -> void: pass

##
# Provides access to log debug information in both backend and frontend.
##
func log_debug(message: String) -> void:
	if OS.has_feature("debug"):
		print(message)

##
# Provides access to log information in both backend and frontend.
##
func log_status(message: String) -> void:
	print(message)

##
# Gets the current active networking socket. Generally used for manipulation of the active socket connections.
#
# Returns: A Node that looks after P2P multiplayer networking.
##
func get_socket() -> ENetMultiplayerPeer:
	return multiplayer.multiplayer_peer