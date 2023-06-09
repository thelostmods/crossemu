class_name WebSocketClient extends Node

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


var _socket          : WebSocketPeer       = WebSocketPeer.new()
var _previous_state  : WebSocketPeer.State = WebSocketPeer.STATE_CLOSED

signal incoming_message(data: Variant)
signal connect_success
signal connect_failed
signal disconnected

func connect_to(
	url:                     String,
	headers:                 PackedStringArray = [],
	protocols:               PackedStringArray = [],
	verify_tls:              bool = false,
	trusted_tls_certificate: X509Certificate = null
) -> int:
	if headers != null:   _socket.handshake_headers = headers
	if protocols != null: _socket.supported_protocols = protocols
	
	# generate tls options data
	var options : TLSOptions = (
		null if not verify_tls else \
		TLSOptions.client(trusted_tls_certificate)
	)
	
	var err_code := _socket.connect_to_url(url, options)
	_previous_state = _socket.get_ready_state()
	set_process(true)
	return err_code

func close(code: int = 1000, reason: String = "") -> void:
	_socket.close(code, reason)

func get_socket() -> WebSocketPeer: return _socket

func get_ready_state() -> WebSocketPeer.State: return _socket.get_ready_state()

func send(data: Variant) -> int:
	if typeof(data) is TYPE_STRING:
		return _socket.send_text(data)
	return _socket.send(var_to_bytes(data))

func _ready() -> void: set_process(false)

func _process(delta: float) -> void:
	var state : WebSocketPeer.State = _socket.get_ready_state()
	
	if state != WebSocketPeer.STATE_CLOSED: _socket.poll()
	
	match (state):
		WebSocketPeer.STATE_CONNECTING:
			_previous_state = state
		
		WebSocketPeer.STATE_OPEN:
			# alert connection succeeded
			if _previous_state == WebSocketPeer.STATE_CONNECTING:
				connect_success.emit()
			_previous_state = state
			
			# parse incoming data
			while _socket.get_available_packet_count():
				incoming_message.emit(_socket.get_packet())
		
		WebSocketPeer.STATE_CLOSING:
			# alert connection failed
			if _previous_state == WebSocketPeer.STATE_CONNECTING:
				connect_failed.emit(
					_socket.get_close_code(),
					_socket.get_close_reason()
				)
			_previous_state = state
		
		WebSocketPeer.STATE_CLOSED:
			# alert connection failed
			if _previous_state == WebSocketPeer.STATE_CONNECTING:
				disconnected.emit()
			_previous_state = state
			
			# disable processing again
			set_process(false)
