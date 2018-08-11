# ChewieBot
### A Twitch Bot for Musicians

---

[Commands][]

<div id="commands"></div>

# Commands
---
[Introduction][CommandIntro]
[Python Scripts][CommandScripts]
[Services Available in Python Scripts][CommandServices]
- [Twitch Service][TwitchService]
- [User Service][UserService]
- [Song Queue Service][SongQueueService]
- [Chat Event Service][ChatEventService]
- [Quote Service][QuoteService]

<div id="commandIntro"></div>

In ChewieBot, commands are written in python scripts.

To create a new command, create a new file in the folder `Commands/CommandScripts`
Python scripts are required to be in the format `nameCommand.py` where `name` is the name of the command. This is what will be used by users in Twitch Chat to execute the command.

`pointsCommand.py` would be executed when a user in the chat would use the command `!points`.

## Python Scripts

<div id="commandScripts"></div>

Command scripts are in the following formats.

#### Chat Commands

```python
# parameters is a dictionary containing all parameters for the command. The order that parameters are in the dictionary is the same order that the parameters will be required in the chat command.
# The parameterName will be used to access the parameter
# bool is used to state whether the parameter is required for the command to be executed.
parameters = { "parameterName": bool }

# This is the point cost to execute the command.
cost = int 

# The minimum user level to execute this command. This should match the name of a User Level in the bot.
minimumUserLevel = "UserLevelName"

# The minimum VIP level to execute this command. This should match the name of a VIP Level in the bot.
minimumVIPLevel = "VIPLevelName"

# The execute function will be called whenever a user uses this command in Twitch Chat and has the required points if needed.
# "params" needs to be an optional argument if all parameters defined earlier are optional. If there are no parameters defined, remove this argument.
# username is the name of the user who called the command in Twitch Chat.
def execute(username, params = None):
    # Code here
    
    # To access a command parameter that's been passed to the function
    variable = params.parameterName
```

##### *Notes*
- All of the variables in the script are optional, and can be removed if they don't have values.
- `params` properties are all strings, as they are parsed from the users chat message.

#### Event Scripts
```python
# State whether this command should be executed on an event triggering.
triggeredOnEvent = bool

# An array with a list of events to register to. 
# The ServiceName is the service that is exposed to python.
# The EventName is the name of the event to register to in the service.
eventsToRegister = ["ServiceName.EventName", "ServiceName.EventName"]

# eventArgs will contain data passed from the registered event.
def execute(eventArgs):
    # Code here
```
##### *Notes*
- Multiple events can be registered to, and the execute function will be triggered for all events registered to.
- Multiple commands can register to the same events.
- There is currently no limit to the number of events a single command can register to, or the number of commands a single event can have registered.

<div id="commandServices"></div>

## Services Available in Python
There are a number of services available to be used within python scripts to make commands easier and more flexible.

- [Twitch Service][TwitchService]
- [User Service][UserService]
- [Song Queue Service][SongQueueService]
- [Chat Event Service][ChatEventService]
- [Quote Service][QuoteService]

#### Response
All services will reteurn a response object, that contains the status of the function call and any data returned from the function call. 

```csharp
public class ScriptServiceResponse
{
    public ScriptServiceResult ResultStatus { get; set; }
    public object Data { get; set; }
    public string Message { get; set; }
}

public enum ScriptServiceResult
{
    SUCCESS,
    USER_NOT_EXIST,
    ERROR,
    PARSE_ERROR
}
```

<div id="twitchService"></div>

### TwitchService
The TwitchService makes functionality related to Twitch available in python scripts.

*Note - The return type from the service functions will be set in the `ScriptServiceResponse.Data` property. If the return type is `void`, the `ScriptServiceResponse.Data` property will be `null`.*

#### Events
##### StreamUpEvent
Triggered when the Twitch PubSub API triggers a StreamUp event.
```csharp
EventHandler<StreamUpArgs> TwitchService.OnStreamUpEvent;

public class StreamUpArgs 
{
    public int PlayDelay;
    public int ServerTime;
}
```
##### StreamDownEvent
Triggered when the Twitch PubSub API triggers a StreamDown event
```csharp
EventHandler<StreamDownArgs> TwitchService.OnStreamDownEvent;

public class StreamDownArgs 
{
    public string ServerTime
}
```
##### ChannelSubscriptionEvent
Triggered when there is a channel subscription.
```csharp
EventHandler<ChannelSubscriptionArgs> TwitchService.OnChannelSubscriptionEvent;

public class ChannelSubscriptionArgs
{
    public ChannelSubscription Subscription;
}

pupblic class ChannelSubscription
{
    public string ChannelId;
    public string ChannelName;
    public string Context;
    public string DisplayName;
    public int Months;
    public string RecipientDisplayName;
    public string RecipientId;
    public string RecipientName;
    public SubMessage SubMessage;
    public SubscriptionPlan SubscriptionPlan;
    public string SubscriptionPlanName;
    public DateTime Time;
    public string UserId;
    public string Username;
}

public class SubMessage
{
    public string Message;
    public List<Emote> Emotes
}

public class Emote
{
    public int Id;
    public int Start;
    public int End;
}

public enum SubscriptionPlan
{
    NotSet = 0,
    Prime = 1,
    Tier1 = 2,
    Tier2 = 3,
    Tier3 = 4
}
```
##### BitsReceivedEvent
Triggers when there are bits received.
```csharp
EventHandler<BitsReceivedArgs> TwitchService.OnBitsReceivedEvent;

public class BitsReceivedArgs
{
    public int BitsUsed;
    public string ChannelId;
    public string ChannelName;
    public string ChatMessage;
    public string Context;
    public string Time;
    public int TotalBitsUsed;
    public string UserId;
    public string Username;
}
```
##### HostEvent
Triggered when there is a host.
```csharp
EventHandler<HostArgs> TwitchService.OnHostEvent;

public class HostArgs
{
    public string HostedChannel;
    public string Moderator;
}
```
##### Functions
To send a message to Twitch Chat.
```csharp
void TwitchService.SendMessage(string message);
```

<div id="userService"></div>

- [Twitch Service][TwitchService]
- [User Service][UserService]
- [Song Queue Service][SongQueueService]
- [Chat Event Service][ChatEventService]
- [Quote Service][QuoteService]

### UserService
The UserService makes a number of functions available to interact with user data.

##### Functions
To get a user object by username.
```csharp
User GetUser(string username);
```
To get the points for a user by username.
```csharp
double GetPointsForUser(string username);
```
To add points to a user by username.
The `points` argument is a `string` to avoid needing to parse chat parameters that would interact with user points.
You can remove points by passing a negative value.
If the `points` argument cannot be parsed to an integer, the `ScriptServiceResponse.ResultStatus` will be `ScriptResultStatus.PARSE_ERROR`.
```csharp
void AddPointsForUser(string username, string points);
```

##### Models
```csharp
public class User
{
    public int Id;
    public string Username;
    public double Points;
    public UserLevel UserLevel;
    public ModLevel ModLevel;
    public VIPLevel VIPLevel;
    public DateTime VIPExpiryDate;
    public double WatchTimeHours;
    public bool IsWatching;
    public DateTime JoinedDate;
}

public class UserLevel
{
    public int Id;
    public string Name;
    public float PointMultiplier;
    public int Rank;
}
public class ModLevel
{
    public int Id;
    public string Name;
    public int Rank;
}

public class VIPLevel
{
    public int Id;
    public string Name;
    public float PointMultiplier;
    public int Rank;
}
```

<div id="songQueueService"></div>

- [Twitch Service][TwitchService]
- [User Service][UserService]
- [Song Queue Service][SongQueueService]
- [Chat Event Service][ChatEventService]
- [Quote Service][QuoteService]

### SongQueueService
The SongQueueService makes functions available to interact with the Song Queue system.

##### Functions
To add a song to the Song Queue with a URL.
```csharp
// Adds a song to the bots song queue. This should only be a valid Youtube URL for now, but other urls will be valid in the future.
Song AddSong(string username, string url)
```

##### Models
```csharp
public class Song
{
    public int Id;
    public string Url;
    public User RequestedBy;    // See the UserService for the Model definition for User.
    public SongRequestType RequestType;
    public SongSourceType SourceType;
    public string Title;
    public TimeSpan Duration;
    public bool Embeddable;
    public bool HasPlayed;
}

// This isn't fully implemented, so for now all songs will be set to Raffle.
public enum SongRequestType
{
    Donation,
    BitDonation,
    NewSubscriber,
    Raffle,
    Points
}

// Only Youtube is a valid source type at the moment, others will be used in the future.
public enum SongSourceType
{
    Invalid,
    Youtube,
    Soundcloud,
    Spotify
}
```

<div id="chatEventService"></div>

- [Twitch Service][TwitchService]
- [User Service][UserService]
- [Song Queue Service][SongQueueService]
- [Chat Event Service][ChatEventService]
- [Quote Service][QuoteService]

## ChatEventService
ChatEventService makes available functions to interact with the Chat Event System

#### Events
##### ChatEventStarted
Triggered whenever a new Chat Event has started.
```csharp
EventHandler<ChatEventStartedEventArgs> OnChatEventStartedEvent;

public class ChatEventStartedEventArgs
{
    public int EventId;
}
```

##### ChatEventEnded
Triggered whenever a Chat Event has ended.
```csharp
EventHandler<ChatEventEndedEventArgs> OnChatEventEndedEvent;

public class ChatEventEndedEventArgs
{
    public ChatEvent ChatEvent;
    public List<EventWinner> EventWinners;
}
``` 

#### Functions
To add a user to an event with a specific `eventId`.
The `ScriptServiceResponse.ResultStatus` will be `ScriptServiceResult.PARSE_ERROR` if the `eventId` cannot be parsed to an `int`.
```csharp
void AddUserToEvent(string eventId, string username);
```

Add a user to a current event of a specific `EventType`.
The `ScriptServiceResponse.ResultStatus` will be `ScriptServiceResult.PARSE_ERROR` if the `eventType` cannot be parsed to a `EventType` enum value.
```csharp
void AddUserToCurrentEvent(string eventType, string username);
```

Create a new event.
The `ScriptServiceResponse.ResultStatus` will be `ScriptServiceResult.PARSE_ERROR` if the `eventType` cannot be parsed to a `EventType` enum value or the `eventDuration` cannot be parsed to an `int`
```csharp
ChatEvent CreateNewEvent(string eventType, string eventDuration);
```

Start an event.
The `ScriptServiceResponse.ResultStatus` will be `ScriptServiceResult.PARSE_ERROR` if the `eventId` cannot be parsed to an `int`.
```csharp
void StartEvent(string eventId);
```

#### Models
```chsarp
public class ChatEvent
{
    public int EventId;
    public EventType Type;
    public DateTime TimeFinished;
    public int Duration;
    public List<User> UserList;
    public bool HasStarted;
    public bool HasFinished;
}

public enum EventType
{
    Raffle,
    Heist,
    Tournament
}
```

<div id="quoteService"></div>

- [Twitch Service][TwitchService]
- [User Service][UserService]
- [Song Queue Service][SongQueueService]
- [Chat Event Service][ChatEventService]
- [Quote Service][QuoteService]

## QuoteService
The QuoteService makes available functions to interact with the Quote system.

#### Functions
Add a new quote.
```csharp
Quote AddQuote(string username, string quotedUser, string quoteText);
```

Get a quote by its id.
```csharp
Quote GetQuote(string id);
```

Delete a quote.
```csharp
void DeleteQuote(string id);
```

#### Models
```csharp
public class Quote
{
    public int Id;
    public User User;
    public string QuoteText;
    public DateTime QuoteTime;
}
```

[Introduction][CommandIntro]
[Python Scripts][CommandScripts]
[Services Available in Python Scripts][CommandServices]
- [Twitch Service][TwitchService]
- [User Service][UserService]
- [Song Queue Service][SongQueueService]
- [Chat Event Service][ChatEventService]
- [Quote Service][QuoteService]

[Commands]: #commands "Commands"
[CommandIntro]: #commandIntro "Introduction"
[CommandScripts]: #commandScripts "Command Scripts"
[CommandServices]: #commandServices "Services Available in Python Scripts"
[TwitchService]: #twitchService
[UserService]: #userService
[SongQueueService]: #songQueueService
[ChatEventService]: #chatEventService
[QuoteService]: #quoteService