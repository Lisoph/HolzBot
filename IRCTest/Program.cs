using System;
using System.Net;
using System.Text.RegularExpressions;

namespace IRCTest
{
  class MainClass
  {
    public static string HostnameToIp(string hostname)
    {
      IPHostEntry entry = Dns.GetHostEntry(hostname);
      return entry.AddressList[0].ToString();
    }
    
    public static void Main()
    {
      Console.WriteLine("Ready");
      Console.Write("Please enter the channel: ");
      string channel = Console.ReadLine();
      
      IPEndPoint twitch = new IPEndPoint(IPAddress.Parse(HostnameToIp("irc.twitch.tv")), 6667);
      //IPEndPoint local = new IPEndPoint(IPAddress.Loopback, 6667);
      
      //IrcClient client = new IrcClient(twitch);
      HolzBot client = new HolzBot(twitch);
      if(!client.Connected)
      {
        Console.WriteLine("Connection failed!");
        client.Close();
        return;
      }
      
      //client.CommandHandlers.Add(new Tuple<Regex, IrcClient.IrcCommandHandler>(IrcRegex.Ping, (Match match, string command) => {}));
      
      /*Console.Write("Connected. Please enter your username: ");
      string username = Console.ReadLine();*/
      
      client.TwitchLogin("HolzBot", "oauth:p5oqngsh0e00es3uikn3lo9zraczsz");
      //client.Login(username);
      
      client.Join(channel);
      client.RunCommand("", "greet");
      
      string input = Console.ReadLine();
      
      while(input.ToLower() != "exit")
      {
        client.SendMessageToChannel(channel, input);
        input = Console.ReadLine();
      }
     
      client.Quit();
      client.Close();
    }
  }
}
