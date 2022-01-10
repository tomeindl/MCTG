using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using Server.Models;
using Server.DAL;
using Server.Utility;

using Npgsql;
using System.Text.Json;
using System.Collections.Generic;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Simple HTTP-Server!");
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            //Populate DB if less than 70 Cards
            //CSV musst be named "cards.csv", have ";" as delimiter and be located next to Server.exe
            if (CardAccess.countFreeCards() < 70)
            {
                List<AbstractCard> list = CSVReader.ReadCards();
                CardAccess.AddCards(list);
            }


            new HttpServer(8080).Run();
        }
    }
}
