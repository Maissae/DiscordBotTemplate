﻿using System;
using System.Threading.Tasks;
using NetCoreBot.Common.Classes;
using NetCoreBot.Common.Interfaces;
using NetCoreBot.Repository.Classes;
using NetCoreBot.Repository.Interfaces;

//TODO: Fix up crashes on unauthorized connection. (Invalid token)
//TODO: Should I use Singleton for settings to avoid passing token through app?
//TODO: Check out CQRS pattern.
//TODO: Add log handler.
//TODO: Add Converter.
//TODO: Add ExceptionHandler.
//TODO: Add CommandHandler.
//TODO: Add Updater.
//TODO: Add Downloader.
//TODO: Add CheckUpdates.
//TODO: Add Cleaner.
//TODO: TESTS!

namespace NetCoreBot
{
    class Program
    {   
        static IConnectionManager connectionHandler = new ConnectionManager(Settings.Instance);
        static IMessageHandler messageHandler = new MessageHandler(connectionHandler, Settings.Instance);
        static ILogHandler logHandler = new LogHandler(connectionHandler, Settings.Instance);

        public static void Bot() => new Bot(
            connectionHandler, 
            Settings.Instance, 
            messageHandler
            
            ).MainAsync().GetAwaiter().GetResult();
        public static void Terminal() => new Terminal().Main();
        static Task bot = new Task(Bot);
        static Task terminal = new Task(Terminal);

        static void Main(string[] args)
        {
            bot.Start();
            terminal.Start();
            bot.Wait();
            terminal.Wait();
        }
    }
}
