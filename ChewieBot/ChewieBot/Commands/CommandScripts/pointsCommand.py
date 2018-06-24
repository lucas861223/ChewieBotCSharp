def execute(username):
	result = UserService.GetPointsForUser(username)
	TwitchService.SendMessage("%s - %d" % (username, result.Data))