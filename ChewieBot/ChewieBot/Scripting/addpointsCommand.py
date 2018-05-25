import clr
clr.AddReferenceToFileAndPath("ChewieBot.Scripting.dll")
from ChewieBot.Scripting import *

def execute(username, params):
	result = UserService.AddPointsForUser(params.username, params.points)
	response = CommandResponse()
	response.message = "%s added %d points to %s" % (username, params.points, params.username)
	print response.message
	return response
    

class CommandResponse:
	message = ""