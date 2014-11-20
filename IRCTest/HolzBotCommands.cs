using System;
using System.Collections.Generic;

namespace IRCTest
{
  public partial class HolzBot
  {
    public delegate void CommandBrain(HolzBot holzBot, string user, string[] args);
    
    public static Tuple<string, CommandBrain>[] commands = new Tuple<string, CommandBrain>[]
    {
      new Tuple<string, CommandBrain>("testcommand", (HolzBot holzBot, string user, string[] args) =>
      {
        holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, "Test 123 Kappa");
      }),
      
      new Tuple<string, CommandBrain>("whoami", (HolzBot holzBot, string user, string[] args) =>
      {
        holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, "I'm HolzBot, a bot developed and maintained by Singender_Holzkuebel!");
      }),
      
      new Tuple<string, CommandBrain>("commands", (HolzBot holzBot, string user, string[] args) =>
      {
        string msg = "Listing commands: ";
        
        for(int i = 0; i < commands.Length; ++i)
        {
          msg += "!" + commands[i].Item1 + (i < commands.Length - 1 ? ", " : "");
        }
        
        holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, msg);
      }),
      
      new Tuple<string, CommandBrain>("whoamiwatching", (HolzBot holzBot, string user, string[] args) =>
      {
        holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, "I'm watching " + holzBot.OnChannel);
      }),
      
      new Tuple<string, CommandBrain>("pause", (HolzBot holzBot, string user, string[] args) =>
      {
        holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, "Zzzz ...");
        holzBot.Paused = true;
      }),
      
      new Tuple<string, CommandBrain>("unpause", (HolzBot holzBot, string user, string[] args) =>
      {
        holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, "... I, uh, totally didn't sleep Kappa");
        holzBot.Paused = false;
      }),
      
      new Tuple<string, CommandBrain>("numviewers", (HolzBot holzBot, string user, string[] args) =>
      {
        holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, "There are " + holzBot.NumViewers + " viewers watching.");
      }),
      
      new Tuple<string, CommandBrain>("greet", (HolzBot holzBot, string user, string[] args) =>
      {
        holzBot.SendMessageToChannel(holzBot.OnChannel, "Hello there! I'm HolzBot, type !commands to get a list of commands!");
      }),
      
      new Tuple<string, CommandBrain>("ping", (HolzBot holzBot, string user, string[] args) =>
      {
        holzBot.SendMessageToChannel(holzBot.OnChannel, "!pong");
      }),
      
      new Tuple<string, CommandBrain>("pong", (HolzBot holzBot, string user, string[] args) =>
      {
        holzBot.SendMessageToChannel(holzBot.OnChannel, "!ping");
      }),
      
      new Tuple<string, CommandBrain>("help", (HolzBot holzBot, string user, string[] args) =>
      {
        if(args == null || args.Length <= 0)
        {
          holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, "Invalid syntax. Type: '!help &lt;command&gt;'");
          return;
        }
        
        string cmd = args[0];
        string help = GetCommandHelp(cmd);
        
        if(help == null)
        {
          holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, "No help found, sorry!");
          return;
        }
        
        holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, cmd + " - " + help);
      }),
      
      new Tuple<string, CommandBrain>("testarg", (HolzBot holzBot, string user, string[] args) =>
      {
        if(args == null || args.Length <= 0)
        {
          holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, "You didn't pass anything!");
          return;
        }
        
        string passed = "";
        for(int i = 0; i < args.Length; ++i)
        {
          passed += args[i] + (i == args.Length - 1 ? "" : ", ");
        }
        
        holzBot.SendMessageToChannelAndUser(holzBot.OnChannel, user, "You passed: " + passed);
      }),
    };
  }
}

