triggeredOnEvent = True
eventsToRegister = ["TwitchService.OnStreamUpEvent", "TwitchService.OnStreamDownEvent"]

def execute(eventArgs):
	TwitchService.SendMessage(eventArgs.EventName)