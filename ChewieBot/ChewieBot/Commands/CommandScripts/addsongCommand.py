parameters = ["url"]
cost = 50

def execute(username, params):
	response = SongQueueService.AddSong(username, params.url)
	TwitchService.SendMessage(response.Message)