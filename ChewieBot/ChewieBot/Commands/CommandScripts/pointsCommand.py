def execute(username):
	result = UserService.GetPointsForUser(username)
	print "%s - %d" % (username, result.Data)