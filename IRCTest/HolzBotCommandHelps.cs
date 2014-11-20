using System;

namespace IRCTest
{
  public partial class HolzBot
  {
    public static string GetCommandHelp(string command)
    {
      foreach(var cmdHelp in commandHelps)
      {
        if(cmdHelp.Item1 == command)
          return cmdHelp.Item2;
      }
      
      return null;
    }
    
    public static Tuple<string, string>[] commandHelps = new Tuple<string, string>[]
    {
      new Tuple<string, string>("testcommand", "Just a test command."),
      new Tuple<string, string>("whoami", "Who is HolzBot?"),
      new Tuple<string, string>("commands", "Makes me type out all the available commands."),
      new Tuple<string, string>("whoamiwatching", "This just prints which channel I'm on."),
      new Tuple<string, string>("pause", "Pauses me."),
      new Tuple<string, string>("unpause", "Un-pauses me."),
      new Tuple<string, string>("numviewers", "The number of all currently watching / lurking viewers."),
      new Tuple<string, string>("greet", "Makes me do the initial on-channel-join greet."),
      new Tuple<string, string>("ping", "( ͡° ͜ʖ ͡°)"),
      new Tuple<string, string>("pong", "( ͡° ͜ʖ ͡°)"),
      new Tuple<string, string>("help", "This is not the help you are looking for."),
      new Tuple<string, string>("testarg", "Command for testing argument passing."),
    };
  }
}

