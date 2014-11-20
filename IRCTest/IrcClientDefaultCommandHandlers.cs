using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IRCTest
{
  public partial class IrcClient
  {
    void RegisterCommandHandlers()
    { 
      foreach(var handler in defaultCommandHandlers)
      {
        CommandHandlers.Add(handler);
      }
    }
    
    static Tuple<Regex, IrcCommandHandler>[] defaultCommandHandlers = new Tuple<Regex, IrcCommandHandler>[]
    {
      // Pong
      new Tuple<Regex, IrcCommandHandler>(IrcRegex.Ping, (IrcClient client, Match match, string message) =>
      {
        client.Write("PONG :" + client.ParseAfterLastColon(message) + "\r\n");
      }),
      
      // Server message
      new Tuple<Regex, IrcCommandHandler>(IrcRegex.ServerMessage, (IrcClient client, Match match, string message) =>
      {
        Console.Write("Server (" + match.Groups[IrcRegexIndex.ServerMessage.ServerName] + ") says:\n\t");
        Console.Write(match.Groups[IrcRegexIndex.ServerMessage.Message] + "\n\t");
        Console.WriteLine("in channel " + match.Groups[IrcRegexIndex.ServerMessage.Channel]);
      }),
      
      // User listing
      new Tuple<Regex, IrcCommandHandler>(IrcRegex.UserListing, (IrcClient client, Match match, string message) =>
      {
        string channel = match.Groups[IrcRegexIndex.UserListing.Channel].ToString();
        Console.Write("Listing users for channel " + (string.IsNullOrWhiteSpace(channel) ? "global" : channel) + "\n\t");

        string[] users = match.Groups[IrcRegexIndex.UserListing.Users].ToString().Split(new char[] {' '});
        for(int i = 0; i < users.Length; ++i)
        {
          Console.Write(users[i] + (i == users.Length - 1 ? "\n" : ", "));
        }
      }),
      
      // User command
      new Tuple<Regex, IrcCommandHandler>(IrcRegex.UserCommand, (IrcClient client, Match match, string message) =>
      {
        string command = match.Groups[IrcRegexIndex.UserCommand.Command].ToString().ToUpper();
        string userName = match.Groups[IrcRegexIndex.UserCommand.UserName].ToString();
        string argument = match.Groups[IrcRegexIndex.UserCommand.Argument].ToString();
        //string target = match.Groups[IrcRegexIndex.UserCommand.Target].ToString();
        string targetArgument = match.Groups[IrcRegexIndex.UserCommand.TargetArgument].ToString();
        
        //Console.WriteLine("Received '" + command + " " + targetArgument + "'");
        
        switch(command)
        {
          case "PRIVMSG":
            client.OnMessageReceived(userName, targetArgument);
            break;
          case "JOIN":
            if(userName == client.UserName)
              client.OnSelfJoin(argument);
            else client.OnJoin(userName, argument);
            break;
          case "PART":
            if(userName == client.UserName)
              client.OnSelfPart(argument);
            else client.OnPart(userName, argument);
            break;
          case "QUIT":
            if(userName == client.UserName)
              client.OnSelfQuit();
            else client.OnQuit(userName);
            break;
          default:
            Console.WriteLine("UNHANDLED COMMAND '" + command + "'");
            Console.WriteLine(message);
            break;
        }
      }),
    };
  }
}

