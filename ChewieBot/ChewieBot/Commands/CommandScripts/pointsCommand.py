parameters = { "username": False }
minimumUserLevel = "Viewer"
minimumVIPLevel = "Gold"

def execute(username, params = None):
	user = username
	if params is not None:
		result = UserService.GetUser(params.username)
		if result.ResultStatus.value__ == ScriptServiceResult.SUCCESS.value__:
			user = result.Data.Username
		else:
			TwitchService.SendMessage("%s doesn't exist." % (params.username))
			return

	result = UserService.GetPointsForUser(user)
	TwitchService.SendMessage("%s - %d" % (user, result.Data))