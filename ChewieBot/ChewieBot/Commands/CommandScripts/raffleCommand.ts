class RaffleCommand {
    parameters: any[]
    eventId: any;

    constructor() {
        this.parameters = new Array<any>("action", "delay", "duration");
    }

    execute(username: string, params: any) {
        switch (params.action) {
            case "start": {
                let response = new CommandResponse();
                this.startRaffle(params.delay, params.duration);
                response.ResponseType = 'Message';
                response.Message = `${username} is starting a raffle!`;
                return response;
            }

            case "join": {
                let response = new CommandResponse();
                response.ResponseType = "Empty";
                this.addUser(username);
                return response;
            }
        }
        
    }

    startRaffle(delay: string, duration: string): void {
        Console.WriteLine(`Starting event raffle`);
        let event = ChatEventService.AddEvent('Raffle', parseInt(delay), parseInt(duration));
        this.eventId = event.EventId;

        ChatEventService.OnEventStarted.connect(function (sender, args) {
            Console.WriteLine(`${this.eventId} -- ${args.ChatEvent.EventId}`);
            if (args.ChatEvent.EventId === event.EventId) {
                Console.WriteLine(`Event ${event.EventId} has started!`);
            }
        });

        ChatEventService.OnEventEnded.connect(function (sender, args) {
            if (args.ChatEvent.EventId === event.EventId) {
                Console.WriteLine(`The winners are...`);
                for (var i = 0; i < args.EventWinners.Count; i++) {
                    Console.WriteLine(`${args.EventWinners[i].User.Username}`);
                }
            }
        });
        
        ChatEventService.StartEvent(parseInt(event.EventId));
    }

    addUser(username: string): void {
        ChatEventService.AddUser(this.eventId, username);
    }
}