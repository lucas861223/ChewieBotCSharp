parameters = ["id"]

def execute(username, params):
	response = QuoteService.DeleteQuote(params.id)
	print response.ResultStatus
	if (response.ResultStatus.value__ == ScriptServiceResult.SUCCESS.value__):
		TwitchMessage.SendMessage("Deleted quote with id %d" % (params.id))