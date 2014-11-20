using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;

namespace IRCTest
{
  public partial class HolzBot: IrcClient
  {
    static Regex commandRegex = new Regex(@"^!(.+)");
    
    bool paused = false;
    int numViewers;
    
    public bool Paused
    {
      get { return paused; }
      set { paused = value; }
    }
    
    public int NumViewers
    {
      get { return numViewers; }
    }
    
    public HolzBot(IPEndPoint server)
      : base(server)
    {
      
    }
    
    public bool RunCommand(string fromUser, string commandStr)
    {
      string[] commandData = commandStr.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
      string cmd = commandData[0];
      
      if(paused && commandStr != "unpause") return false;

      foreach(var command in commands)
      {
        if(command.Item1 == cmd)
        {
          string[] args = null;
          
          if(commandData.Length > 1)
          {
            args = new string[commandData.Length - 1];
            Array.Copy(commandData, 1, args, 0, commandData.Length - 1);
          }
          
          command.Item2(this, fromUser, args);
          Console.WriteLine("Ran command '" + commandStr + "' for '" + fromUser + "'");
          return true;
        }
      }
      
      return false;
    }
    
    #region Events
    public override void OnMessageReceived(string fromUser, string message)
    {
      Match match = commandRegex.Match(message);
      if(!match.Success)
      {
        Console.WriteLine("Not a command: " + message);
        return;
      }
      
      string commandStr = match.Groups[1].ToString().Trim().ToLower();
      RunCommand(fromUser, commandStr);
    }
    
    public override void OnJoin(string user, string channel)
    {
      ++numViewers;
    }
    
    public override void OnPart(string user, string channel)
    {
      numViewers = Math.Max(numViewers - 1, 0);
    }
    #endregion
  }
}

