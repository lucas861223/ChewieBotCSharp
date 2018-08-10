parameters = { "id": True }

def execute(username, params):
	response = QuoteService.GetQuote(params.id)
	if (response.Data):
		TwitchService.SendMessage("'%s' - %s (%s-%s-%s)" % (response.Data.QuoteText, response.Data.User.Username, response.Data.QuoteTime.Year, response.Data.QuoteTime.Month, response.Data.QuoteTime.Day))