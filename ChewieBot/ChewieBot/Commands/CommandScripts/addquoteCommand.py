parameters= { "user": True, "quoteText": True }

def execute(username, params):
	response = QuoteService.AddQuote(username, params.user, params.quoteText)
	if (response.Data):
		TwitchService.SendMessage("Quote added with id: %d" % response.Data.Id)