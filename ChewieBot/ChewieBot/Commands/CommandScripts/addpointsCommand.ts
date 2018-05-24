class AddpointsCommand {
    execute(username: string, data: any) {
        Console.WriteLine(`Data: ${data}`);
        var result = UserService.AddPointsForUser(data.Username, data.Points);
        var response = new CommandResponse(`${username} added ${data.Points} to ${data.Username}!`);
        return response;
    }
}