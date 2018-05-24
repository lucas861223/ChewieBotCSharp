class PointsCommand {
    execute(username: string, data: any) {
        Console.WriteLine(`Data: ${data}`);
        var result = UserService.GetUser(username);
        Console.WriteLine(`Result: ${result}`);
        var response = new CommandResponse(`${result.Data.Username} - ${result.Data.Points}`);
        return response;
    }
}