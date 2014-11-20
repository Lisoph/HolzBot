using System;
using System.Text.RegularExpressions;

namespace IRCTest
{
  public static class IrcRegex
  {
    public static Regex LastColon = new Regex(@"^.+:(.*)");
    public static Regex Ping = new Regex(@"^PING.*");
    public static Regex ServerMessage = new Regex(@"^:(\S+)\s(\d+)\s(\S+)\s(#(\S+)\s)?:(.*)");
    public static Regex UserListing = new Regex(@"^:(\S+)\s(\d+)\s(\S+)\s=\s(#(\S+)\s)?:(.*)");
    public static Regex UserCommand = new Regex(@"^:(\S+)!(\S+)@(\S+)\s(\w+)\s(((#?\S+)\s:(.*))|(#?:?(\S+)))");
  }
  
  public static class IrcRegexIndex
  {
    public static class ServerMessage
    {
      public const int ServerName = 1;
      public const int Id = 2;
      public const int To = 3;
      public const int Channel = 5;
      public const int Message = 6;
    }
    
    public static class UserListing
    {
      public const int ServerName = 1;
      public const int Id = 2;
      public const int To = 3;
      public const int Channel = 5;
      public const int Users = 6;
    }
    
    public static class UserCommand
    {
      public const int UserName = 1;
      public const int UserNick = 2;
      public const int UserIp = 3;
      public const int Command = 4;
      public const int Argument = 10;
      public const int Target = 7;
      public const int TargetArgument = 8;
    }
  }
}

