parameters = ["eventType", "duration"]

def execute(username, params):
	response = ChatEventService.CreateNewEvent(params.eventType, params.duration)
	ChatEventService.OnEventStarted += eventStarted
	ChatEventService.OnEventEnded += eventEnded
	ChatEventService.StartEvent(response.Data.EventId)

def eventStarted(sender, args):
	print "Event %d has started!" % (args.EventId)

def eventEnded(sender, args):
	print "Event %d has ended!" % (args.ChatEvent.EventId)
	print "The winner was %s!" % (args.EventWinners[0].User.Username)