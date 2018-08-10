parameters = { "username": True, "points": True }

def execute(username, params):
	result = UserService.AddPointsForUser(params.username, params.points)
	return "%s added %s points to %s" % (username, params.points, params.username)