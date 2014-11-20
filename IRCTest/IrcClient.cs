using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace IRCTest
{
  public partial class IrcClient: IIrcCommands
  {
    #region Members
    TcpClient tcpClient;
    Thread brainThread;
    
    bool socketConnected = false;
    bool allOk = true;
    
    public delegate void IrcCommandHandler(IrcClient client, Match match, string message);
    
    List<Tuple<Regex, IrcCommandHandler>> commandHandlers = new List<Tuple<Regex, IrcCommandHandler>>();
    
    string userName = "";
    string onChannel = "";
    #endregion
    
    #region General class stuff
    public IrcClient(IPEndPoint server)
    {
      tcpClient = new TcpClient();
      
      try
      {
        tcpClient.Connect(server);
      }
      catch(SocketException)
      {
        Console.WriteLine("IrcClient ctor exception!");
      }
      
      socketConnected = tcpClient.Connected;
      if(!Connected) return;
      
      RegisterCommandHandlers();
      
      brainThread = new Thread(Brain);
      brainThread.Start();
    }
    
    public void Dispose()
    {
      tcpClient.Close();
      
      allOk = false;
      if(brainThread != null)
        brainThread.Join();
    }
    
    public void Close()
    {
      Dispose();
    }
    #endregion
    
    #region Properties
    public bool Connected
    {
      get { return socketConnected; }
    }
    
    public List<Tuple<Regex, IrcCommandHandler>> CommandHandlers
    {
      get { return commandHandlers; }
    }
    
    public string UserName
    {
      get { return userName; }
    }
    
    public string OnChannel
    {
      get { return onChannel; }
    }
    #endregion
    
    #region Internal Logic
    void Brain()
    {
      NetworkStream netStream;
      
      lock(tcpClient)
      {
        netStream = tcpClient.GetStream();
      }
      
      while(allOk)
      {
        int avail = tcpClient.Available; // Maybe access this in a lock.
        if(avail > 0)
        {
          byte[] data = new byte[avail];
          netStream.Read(data, 0, avail);
          
          string dataStr = new string(System.Text.Encoding.UTF8.GetChars(data));
          HandleData(dataStr);
        }
        
        if(allOk) Thread.Sleep(33);
      }
    }
    
    void HandleData(string data)
    {
      string[] lines = data.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
      
      foreach(string line in lines)
      {
        line.Trim();
        
        bool handlerFound = false;
        
        foreach(var handler in CommandHandlers)
        {
          Match match = handler.Item1.Match(line);
          if(match.Success)
          {
            handler.Item2(this, match, line);
            handlerFound = true;
            break;
          }
        }
        
        if(!handlerFound)
        {
          Console.WriteLine("Unhandled message -> " + line);
        }
      }
    }
    
    public string ParseAfterLastColon(string message)
    {
      Match match = IrcRegex.LastColon.Match(message);
      
      if(!match.Success) return null;
      return match.Groups[1].Value;
    }
    
    public void Write(string data)
    {
      lock(tcpClient)
      {
        NetworkStream stream = tcpClient.GetStream();
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes(data);
        if(stream.CanWrite)
          stream.Write(rawData, 0, rawData.Length);
      }
    }
    #endregion
    
    #region IRC Commands implementation
    public void Nick(string name)
    {
      userName = name;
      Write("NICK " + name + "\r\n");
    }
    
    public void User(string name)
    {
      Write("USER " + name + " 0 * :_" + name + "\r\n");
    }
    
    public void Pass(string password)
    {
      Write("PASS " + password + "\r\n");
    }
    
    public void Login(string username, string password = null)
    {
      Nick(username);
      User("_" + username);
      if(password != null) Pass(password);
    }
    
    public void TwitchLogin(string username, string password)
    {
      Pass(password);
      Nick(username);
    }
    
    public void SendMessage(string target, string message)
    {
      Write("PRIVMSG " + target + " :" + message + "\r\n");
    }
    
    public void SendMessageToUser(string user, string message) { SendMessage(user, message); }
    public void SendMessageToChannel(string channel, string message) { SendMessage('#' + channel, message); }
    public void SendMessageToChannelAndUser(string channel, string user, string message) { SendMessage('#' + channel, "@" + user + ": " + message); }
    
    public void Quit(string reason = null)
    {
      Write("QUIT" + (reason != null ? " :" + reason : "") + "\r\n");
    }
    
    public void Join(string channel)
    {
      Write("JOIN :#" + channel + "\r\n");
      onChannel = channel;
    }
    
    public void Part(string channel, string reason = null)
    {
      Write("PART #" + channel + (reason != null ? " :" + reason : "") + "\r\n");
    }
    #endregion
    
    #region Events / Callbacks
    public virtual void OnMessageReceived(string fromUser, string message) {}
    public virtual void OnSelfJoin(string channel) {}
    public virtual void OnJoin(string user, string channel) {}
    public virtual void OnSelfPart(string channel) {}
    public virtual void OnPart(string user, string channel) {}
    public virtual void OnSelfQuit() {}
    public virtual void OnQuit(string user) {}
    #endregion
  }
}

