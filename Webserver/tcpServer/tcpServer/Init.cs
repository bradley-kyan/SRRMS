using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.IO;
using System.Reflection;
using System.Drawing.Imaging;
using System.Drawing;
using System.Threading;
using System.Text.Json;
using System.Collections;
using System.Linq;

namespace tcpServer
{
    class Init
    {
        public const string options = " 1: Setup\n 2: Devices\n 3: Start server\n_____________________________\n\n F12: Exit";
        public static string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string pLocate = currentPath + "/preferences.json";

        public static void startup()
        {
            Setup s = new Setup();

            s.Header(1);

            if (File.Exists(pLocate))
            {
                Console.WriteLine(" preferences.json found\n\n");
            }
            else
            {
                Console.WriteLine(" preferences.json not found\n");
                s.Serializer();
                Console.WriteLine(" Created preferences.json\n\n");
            }
            Thread.Sleep(1000);
            s.OptionSelect();

        }
    }

    public class Prefernces
    {
        public DateTime UpdateTime { get; set; }
        public string ConnectionString { get; set; }
        public string PWD { get; set; }
        public IList<string> DeviceIds { get; set; }
        public IList<string> DeviceNote { get; set; }
        public Prefernces()
        {
            DeviceIds = new List<string>();
            DeviceNote = new List<string>();
        }
    }


    class Setup : Init
    {
        Prefernces p = new Prefernces();

        public void Header(int header)
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
            Header(2);
            ConsoleKeyInfo info = Console.ReadKey(true);

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
                    Console.WriteLine("Pressed 3");
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

        public void Pref()
        {
            Header(1);
            Deserializer();
        }

        public string Serializer()//Writes data to file
        {
            Header(1);

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
            catch(Exception e)
            {
                Console.WriteLine(e +"\n\n ERROR");
                return null;
            }
        }

        public string Deserializer()//Gets data from file
        {
            Header(1);

            try
            {
                var text = File.ReadAllText(pLocate);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                p = JsonSerializer.Deserialize<Prefernces>(text, options);
                string jsonString = JsonSerializer.Serialize(p, options);

                return jsonString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "\n\n ERROR");
                return null;
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

                    if(result.Length != 10)
                    {
                        Console.WriteLine("Invalid Length");
;                       Thread.Sleep(1000);
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

        private bool productVerify(string input)
        {
            string[] keySplit = input.Split("-");
            try
            {
                if (keySplit[0].Length != 4 || keySplit[1].Length != 4 || keySplit[2].Length != 4 || keySplit[3].Length != 4)
                {
                    return false;
                }

                int[] numSplit = new int[4];

                foreach (string s in keySplit)
                {
                    char[] c = s.ToCharArray();

                    int[] numbersRaw = new int[4];

                    for (int i = 0; i <= c.Length; i++)
                    {
                        int index = char.ToUpper(c[i]) - 64;
                        numbersRaw.Append(index);
                    }
                    numSplit.Append(Convert.ToInt32(String.Join("", numbersRaw)));
                }
                Console.WriteLine(numSplit);
                return true;
            }
            catch
            {
                return false;
            }
            

           // Random rand = new Random(DateTime.Now.Millisecond/DateTime.Now.Month);
           // int rand9 = rand.Next(1, 10);

            //divisible by 26 
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
                        if(p.DeviceIds.Remove(result) == true)
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
    }
}
