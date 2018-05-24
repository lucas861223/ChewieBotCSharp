class AddpointsCommand {
    parameters: string[]

    constructor() {
        this.parameters = new Array<string>("username", "points");
    }

    execute(username: string, params: any) {
        Console.WriteLine(`Data: ${params}`);
        var result = UserService.AddPointsForUser(params.username, params.points);
        var response = new CommandResponse(`${username} added ${params.points} to ${params.username}!`);
        return response;
    }
}