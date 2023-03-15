extends Node

#################################
## Packets sent by the Client, ##
## and received by the Server. ##
#################################

enum Client {
	message
}

#################################
## Packets sent by the Server, ##
## and received by the Client. ##
#################################

enum Server {
	response
}

#################################
## Packets sent or received by ##
## a Hybrid Client/Server Net. ##
#################################

enum Hybrid {
	message,
	response
}
