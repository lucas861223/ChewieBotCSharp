class PointsCommand {
    execute(username: string, params: any) {
        var result = UserService.GetUser(username);
        var response = new CommandResponse();
        response.Message = `${result.Data.Username} - ${result.Data.Points}`;
        return response;
    }
}