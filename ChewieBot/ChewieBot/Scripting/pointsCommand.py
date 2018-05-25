import sys
import clr
clr.AddReferenceToFileAndPath("ChewieBot.Scripting.dll");
from ChewieBot.Scripting import *

def execute(username):
	result = UserService.GetPointsForUser(username)
	print "%s - %d" % (username, result.Data)