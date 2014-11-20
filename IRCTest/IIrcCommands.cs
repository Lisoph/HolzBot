using System;

namespace IRCTest
{
  public interface IIrcCommands
  {
    void Nick(string name);
    void User(string name);
    void Pass(string name);
    
    void SendMessage(string target, string message); // PRIVMSG
    
    void Join(string channel);
    void Part(string channel, string reason = null);
    void Quit(string reason = null);
  }
}

