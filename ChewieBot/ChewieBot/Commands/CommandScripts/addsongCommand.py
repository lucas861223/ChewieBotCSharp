parameters = ["url"]

def execute(username, params):
	response = SongQueueService.AddSong(username, params.url)
	TwitchService.SendMessage(response.Message)