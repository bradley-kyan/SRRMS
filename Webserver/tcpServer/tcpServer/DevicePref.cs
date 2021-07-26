using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace tcpServer
{
    public class DevicePref : Init
    {
        public Preferences p = new Preferences();

        public void Header(byte header)
        {
            //Calvin S Font https://patorjk.com/software/taag/#p=display&f=Calvin%20S&t=
            string title = "\n \u2554\u2550\u2557\u2566\u2550\u2557\u2566\u2550\u2557\u2554\u2566\u2557\u2554\u2550\u2557  \u2554\u2550\u2557\u2554\u2550\u2557\u2566\u2550\u2557\u2566  \u2566\u2554\u2550\u2557\u2566\u2550\u2557\r\n \u255A\u2550\u2557\u2560\u2566\u255D\u2560\u2566\u255D\u2551\u2551\u2551\u255A\u2550\u2557  \u255A\u2550\u2557\u2551\u2563 \u2560\u2566\u255D\u255A\u2557\u2554\u255D\u2551\u2563 \u2560\u2566\u255D\r\n \u255A\u2550\u255D\u2569\u255A\u2550\u2569\u255A\u2550\u2569 \u2569\u255A\u2550\u255D  \u255A\u2550\u255D\u255A\u2550\u255D\u2569\u255A\u2550 \u255A\u255D \u255A\u2550\u255D\u2569\u255A\u2550\n";
            switch (header)
            {
                case 1:
                    Console.Clear();
                    Console.Title = "SRRMS Server";
                    Console.WriteLine(title);
                    break;
                case 2:
                    Console.Clear();
                    Console.Title = "SRRMS Server";
                    Console.WriteLine(title);
                    Console.WriteLine(options);
                    break;
                case 3:
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                    break;
                default:
                    break;
            }
        }
        public void OptionSelect()
        {
            var ServerStart = new ServerInitializer();

            Header(2);
            ConsoleKeyInfo info = Console.ReadKey(true);
            Deserializer();

            switch (info.Key)
            {
                case ConsoleKey.D1:
                    Header(3);
                    Console.WriteLine("Pressed 1");
                    Pref();
                    break;
                case ConsoleKey.D2:
                    Header(3);
                    Console.WriteLine("Pressed 2");
                    DeviceManage();
                    break;
                case ConsoleKey.D3:
                    Header(3);
                    PreferencesStatic.DeviceIds = p.DeviceIds.ToArray();
                    PreferencesStatic.UpdateValues(p.DbType, p.ConnectionString, p.DBUpdateTime);
                    ServerStart.Initializer();
                    break;
                case ConsoleKey.F12:
                    Environment.Exit(0);
                    break;
                default:
                    Header(3);
                    Console.WriteLine("Invalid Input");
                    OptionSelect();
                    break;
            }
        }
        public string Serializer()//Writes data to file
        {
            Header(1);
            lock (this)
            {
                p.UpdateTime = DateTime.Now;
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    };

                    string jsonString = JsonSerializer.Serialize(p, options);
                    File.WriteAllText(pLocate, jsonString);
                    return jsonString;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e + "\n\n ERROR");
                    return null;
                }
            }

        }
        public string Deserializer()//Gets data from file
        {
            Header(1);
            lock (this)
            {
                try
                {
                    var text = File.ReadAllText(pLocate);

                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    };
                    p = JsonSerializer.Deserialize<Preferences>(text, options);
                    string jsonString = JsonSerializer.Serialize(p, options);

                    return jsonString;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e + "\n\n ERROR");
                    return null;
                }
            }
        }
        public void DeviceManage()
        {
            Header(1);
            Deserializer();
            Console.WriteLine(" Current Ammount of devices: {0}\n Press ESC to Return\n\n 1: Add Device\n 2: View Devices\n 3: Remove Device\n", DeviceCount());

            ConsoleKeyInfo info = Console.ReadKey(true);

            switch (info.Key)
            {
                case ConsoleKey.D1:
                    AddDevice();
                    break;
                case ConsoleKey.D2:
                    ViewDevices();
                    break;
                case ConsoleKey.D3:
                    RemoveDevice();
                    break;
                case ConsoleKey.Escape:
                    OptionSelect();
                    break;
                case ConsoleKey.F12:
                    Environment.Exit(0);
                    break;
                default:
                    DeviceManage();
                    break;
            }
        }
        public int DeviceCount()
        {
            var none = 0;
            int amount = p?.DeviceIds?.Count ?? none;
            return amount;
        }
        public void ViewDevices()
        {
            bool repeat;

            Header(1);
            Console.WriteLine(" Current Ammount of devices: {0}\n Press ESC to Return\n", DeviceCount());
            Devices();
            do
            {
                ConsoleKeyInfo info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Escape)
                {
                    DeviceManage();
                    repeat = false;
                }
                else
                {
                    repeat = true;
                }
            }
            while (repeat == true);
        }
        public void AddDevice()
        {
            bool repeat = false;
            do
            {
                Header(1);

                string result;
                StringBuilder buffer = new StringBuilder();

                Console.WriteLine(" Enter ID of device then press enter: (ESC to cancel)");

                //The key is read passing true for the intercept argument to prevent
                //any characters from displaying when the Escape key is pressed.
                ConsoleKeyInfo info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Escape)
                {
                    DeviceManage();
                }
                while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
                {
                    if (info.Key == ConsoleKey.Backspace)
                    {
                        Console.Write("\b");
                        Console.Write(" ");
                        Console.Write("\b");
                    }
                    else
                    {
                        Console.Write(info.KeyChar);
                    }
                    buffer.Append(info.KeyChar);
                    info = Console.ReadKey(true);
                }
                if (info.Key == ConsoleKey.Enter)
                {
                    result = buffer.ToString();

                    if (result.Length != 10)
                    {
                        Console.WriteLine("Invalid Length");
                        Thread.Sleep(1000);
                        AddDevice();
                    }

                    Console.WriteLine("\n Is this code correct? => {0} \n Y: Yes\n N: No", result);

                    if (Console.ReadKey().KeyChar.ToString() == "y")
                    {
                        bool state = false;

                        p.DeviceIds.Add(result);

                        do
                        {
                            Console.WriteLine("\nAdd Note?");
                            Console.WriteLine("\n Y: Yes\n N: No");

                            var inpt = Console.ReadKey().KeyChar.ToString();
                            if (inpt == "y")
                            {
                                Console.WriteLine(" Enter Note then press enter:");
                                string note = Console.ReadLine();
                                p.DeviceNote.Add($"{result}  --  {note}");
                                break;
                            }
                            else if (inpt == "n")
                            {
                                p.DeviceNote.Add($"{result}  --  null");
                                break;
                            }

                            Console.WriteLine("Invalid Input");
                            Thread.Sleep(1000);
                            state = true;

                        }
                        while (state == true);
                    }
                    else
                    {
                        repeat = true;
                    }
                }
            }
            while (repeat == true);
            Serializer();
            DeviceManage();
        }
        public void RemoveDevice()
        {
            bool repeat = false;
            do
            {
                Header(1);

                string result = null;
                StringBuilder buffer = new StringBuilder();

                Console.WriteLine(" Enter ID of device (10 digit code) then press enter: (ESC to cancel)");

                //The key is read passing true for the intercept argument to prevent
                //any characters from displaying when the Escape key is pressed.
                ConsoleKeyInfo info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Escape)
                {
                    DeviceManage();
                }
                while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
                {
                    if (info.Key == ConsoleKey.Backspace)
                    {
                        Console.Write("\b");
                        Console.Write(" ");
                        Console.Write("\b");
                    }
                    else
                    {
                        Console.Write(info.KeyChar);
                    }
                    buffer.Append(info.KeyChar);
                    info = Console.ReadKey(true);
                }
                if (info.Key == ConsoleKey.Enter)
                {
                    result = buffer.ToString();

                    if (result.Length != 10)
                    {
                        Console.WriteLine("Invalid Length");
                        Thread.Sleep(1000);
                        RemoveDevice();
                    }

                    Console.WriteLine("\n Is this code correct? => {0} \n Y: Yes\n N: No", result);
                    if (Console.ReadKey().KeyChar.ToString() == "y")
                    {
                        if (p.DeviceIds.Remove(result) == true)
                        {
                            Console.WriteLine(" Successfully Removed Device: {0}", result);

                            var remove = p.DeviceNote.FirstOrDefault(a => a.Contains(result));
                            if (remove != null)
                            {
                                p.DeviceNote.Remove(remove);
                            }

                            Serializer();
                        }
                        else
                        {
                            Header(3);
                            Console.WriteLine(" Invalid ID");
                            Thread.Sleep(1000);
                            repeat = true;
                        }
                    }
                    else
                    {
                        repeat = true;
                    }
                }
            }
            while (repeat == true);
            DeviceManage();
        }
        public void Devices()
        {
            int amount = 1;
            foreach (var item in p.DeviceNote)
            {
                Console.WriteLine($" {amount}: {item}");
                amount++;
            }
        }
        public void Pref()
        {
            var conStr = new ConnectionString();
            conStr.SetDbProvider();
            Header(1);
            Deserializer();
            Console.WriteLine($" Current database provider: {p.DbType}\n Press ESC to Return\n\n 1: Change Database Provider\n 2: View Connection String\n 3: Edit Connection String\n");

            ConsoleKeyInfo info = Console.ReadKey(true);

            switch (info.Key)
            {
                case ConsoleKey.D1:
                    conStr.SetDbProvider(true);
                    break;
                case ConsoleKey.D2:
                    Header(1);
                    Console.WriteLine($" {conStr.GetConString()}\n\n Press Any Key...");
                    Console.ReadKey(true);
                    Pref();
                    break;
                case ConsoleKey.D3:
                    conStr.SetConString();
                    break;
                case ConsoleKey.Escape:
                    OptionSelect();
                    break;
                case ConsoleKey.F12:
                    Environment.Exit(0);
                    break;
                default:
                    Pref();
                    break;
            }
        }
    }
    internal class ConnectionString : DevicePref
    {
        /// <summary>
        /// Sets the database provider from the command line.
        /// <para /> Method can be overloaded with => <c>Bool</c> This is to be used for option selection.
        /// </summary>
        internal void SetDbProvider()
        {
            Deserializer();
            if (p.DbType is null)
            {
                bool repeat;
                do
                {
                    repeat = false;
                    Header(1);
                    Console.Write("Specify Database provider: (Press 1 or 2)\n1 : MySQL (disabled)\n2 : MSSQL");
                    ConsoleKeyInfo keyType = Console.ReadKey(true);
                    var input = keyType.Key;
                    /*if (input == ConsoleKey.D1)
                    {
                        p.DbType = "MySql";
                    }*/
                    if (input == ConsoleKey.D2)
                    {
                        p.DbType = "MSSQL";
                    }
                    else
                    {
                        repeat = true;
                    }

                } while (repeat == true);

                Header(1);
                SetConString();
                Pref();
            }
            if (p.ConnectionString is null)
            {
                Header(1);
                SetConString();
                Pref();
            }
        }
        /// <summary>
        /// Sets the database provider from the command line.
        /// <para /> Method can be overloaded with => <c>Bool</c> This is to be used for option selection.
        /// </summary>
        internal void SetDbProvider(bool get)
        {
            Deserializer();
            string originProvider = p.DbType;
            bool repeat;
            do
            {
                repeat = false;
                Header(1);
                Console.Write(" Specify Database provider: \n Press ESC to return\n\n 1 : MySQL (disabled)\n 2 : MSSQL");
                ConsoleKeyInfo keyType = Console.ReadKey(true);
                var input = keyType.Key;
                /*if (input == ConsoleKey.D1)
                {
                    p.DbType = "MySql";
                }*/
                if (input == ConsoleKey.D2)
                {
                    p.DbType = "MSSQL";
                }
                else if (input == ConsoleKey.Escape)
                {
                    Pref();
                }
                else
                {
                    repeat = true;
                }
            } while (repeat == true);
            if (originProvider == p.DbType)
            {
                Pref();
            }
            Header(1);
            SetConString();
            Pref();
        }
        internal string GetConString()
        {
            Deserializer();
            if (p.ConnectionString is null)
                return "null";
            else
                return p.ConnectionString;
        }
        internal void SetConString()
        {
            Serializer();
            Deserializer();

            bool repeat = false;
            do
            {
                Header(1);
                Console.WriteLine($" Enter connection string for {p.DbType}: (ESC to cancel)\n");

                string result;
                StringBuilder buffer = new StringBuilder();

                //The key is read passing true for the intercept argument to prevent
                //any characters from displaying when the Escape key is pressed.
                ConsoleKeyInfo info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Escape)
                {
                    Pref();
                }
                while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
                {
                    if (info.Key == ConsoleKey.Backspace)
                    {
                        Console.Write("\b");
                        Console.Write(" ");
                        Console.Write("\b");
                    }
                    else
                    {
                        Console.Write(info.KeyChar);
                    }
                    buffer.Append(info.KeyChar);
                    info = Console.ReadKey(true);
                }
                if (info.Key == ConsoleKey.Enter)
                {
                    result = buffer.ToString();

                    Console.WriteLine("\n Is this correct? => {0} \n Y: Yes\n N: No", result);

                    if (Console.ReadKey().KeyChar.ToString() == "y")
                    {
                        p.ConnectionString = result;
                        Serializer();
                        Console.WriteLine(" Successfully created / updated database connection string!\n\n Press Any Key...");
                        Console.ReadKey(true);
                        Pref();
                    }
                    else
                    {
                        repeat = true;
                    }
                }
            }
            while (repeat == true);
        }
    }
}

