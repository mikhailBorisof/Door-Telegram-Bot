using System;
using Newtonsoft.Json.Linq;
using Telegram.Bot; // библиотека ботов тг
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Speech.Synthesis; // библиотека речи
using static System.Net.Mime.MediaTypeNames;
using Telegram.Bot.Types;

namespace Telegram_Bot {
    class Programm {
        // main инициализация 
        static void Main(string[] args) {
            // в консоль
            Console.WriteLine($"Бот сейчас включён. Сервер запущен в {DateTime.Now} \n"); 
            // метод начала диалога с ботом
            Communication.Init();
        }
    }
    class Communication {
        // Initialize a new instance of the TelegramBotClient
        private static TelegramBotClient client;
        // Initialize a new instance of the SpeechSynthesizer
        private static SpeechSynthesizer synth;
        // время delay звонка
        private static int waitTimeSec = 30;
        private static int WaitTimeSec
        {
            get { return waitTimeSec; }
            set
            {
                if(0 <= value && value <= 60) waitTimeSec = value;
                else
                { // в консоль
                    Console.WriteLine("Нельзя установить время ожидания звонка меньше 0 и больше 60 секунд ");
                }
            }
        }
        // часы работы бота
        private static int hourOfStart = 8;
        private static int HourOfStart
        {
            get { return hourOfStart; }
            set
            {
                if(0 <= value && value <= 24) hourOfStart = value;
                else
                { // в консоль
                    Console.WriteLine("Нельзя установить часы работы меньше 0 и больше 24 часов ");
                }
            }
        }
        private static int hourOfStop = 23;
        internal static int HourOfStop
        {
            get { return hourOfStop; }
            set
            {
                if (0 <= value && value <= 24) hourOfStop = value;
                else
                { // в консоль
                    Console.WriteLine("Нельзя установить часы работы меньше 0 и больше 24 часов ");
                }
            }
        }

        private static string token = "token";
        internal static string Token { get { return token; } }
        // private static TelegramBotClient client;
        private static DateTime userTime = DateTime.Now;

        // метод начала диалога с ботом
        internal static void Init() {
            /* только для Windows
            // инициализация новой говорилки
            synth = new SpeechSynthesizer();
            // Configure the audio output.   
            synth.SetOutputToDefaultAudioDevice();
            */
            // Console.WriteLine("Hello, World!");
            client = new TelegramBotClient(Token); // добавение нового бота
            client.StartReceiving(); // начало приема сообщений
            client.OnMessage += OnMessageHandler; // подписываемся на события сообщений
            client.OnMessageEdited += OnMessageHandler; // подписываемся на события изминения сообщений
            Console.ReadLine(); // ждем символа от консоли
            client.StopReceiving(); // конец приема сообщений
        }
        // метод действий на команду /stop
        private static async void Stop(Telegram.Bot.Types.Message msg) {
            // в чат
            await client.SendTextMessageAsync(msg.Chat.Id, "Бот успешно выключен");
            // в беседу
            await client.SendTextMessageAsync("chatID", "Бот успешно выключен");
            // в консоль
            Console.WriteLine("Бот успешно выключен"); Environment.Exit(0);
        }
        // метод действий на команду /start
        private static async void Start(Telegram.Bot.Types.Message msg) {
            // в чат о кнопках
            await client.SendTextMessageAsync(
                msg.Chat.Id, "Здравствуйте!\n\nВы можете нажать на кнопку звонка или написать сообщение",
                replyMarkup: GetButtonsCall());
            // в консоль | имя есть
            if (msg.From != null && msg.Chat.FirstName != null) {
                Console.WriteLine(DateTime.Now);
                Console.WriteLine($"У ворот стоит человек." +
                    $"\nЗашел в чат." +
                    $"\nЕго зовут: {msg.Chat.FirstName}" +
                    $"\nВ телеграме известен как: {msg.From}\n");
            }
            // в беседу и в консоль | имени нет
            else {
                Console.WriteLine(DateTime.Now);
                Console.WriteLine($"У ворот стоит человек." +
                    $"\nЗашел в чат." +
                    $"\nЕго зовут: !Имя скрыто!\n");
            }
        }
        // метод действий на команду /changeTo
        private static async void ChangeTimeTo(Telegram.Bot.Types.Message msg) {
            // в чат о кнопках
            await client.SendTextMessageAsync(
                msg.Chat.Id, "Здравствуйте!\n\nВы можете изменить время \n" +
                "до которого работает звонок," +
                "нажав на кнопку часа до которого будет работать звонок",
                replyMarkup: ChangeTimeToButtons());
            // в консоль | имя есть
            if (msg.From != null && msg.Chat.FirstName != null)
            {
                await client.SendTextMessageAsync(
                       "chatID",
                       $"Человек меняет время до которого работает звонок." +
                       $"\nЕго зовут: {msg.Chat.FirstName}" +
                       $"\nВ телеграме известен как: {msg.From}");
                Console.WriteLine(DateTime.Now);
                Console.WriteLine($"Человек меняет время до которого работает звонок." +
                    $"\nЕго зовут: {msg.Chat.FirstName}" +
                    $"\nВ телеграме известен как: {msg.From}\n");
            }
            // в консоль | имени нет
            else
            {
                await client.SendTextMessageAsync(
                       "chatID",
                       $"Человек меняет время до которого работает звонок." +
                        $"\nЕго зовут: !Имя скрыто!");
                Console.WriteLine(DateTime.Now);
                Console.WriteLine($"Меняет время до которого работает звонок." +
                    $"\nЕго зовут: !Имя скрыто!\n");
            }
        }
        // метод действий на команду /changeFrom
        private static async void ChangeTimeFrom(Telegram.Bot.Types.Message msg) {
            // в чат о кнопках
            await client.SendTextMessageAsync(
                msg.Chat.Id, "Здравствуйте!\n\nВы можете изменить время \n" +
                "от которого работает звонок," +
                "нажав на кнопку часа от которого будет работать звонок",
                replyMarkup: ChangeTimeFromButtons());
            // в консоль | имя есть
            if (msg.From != null && msg.Chat.FirstName != null)
            {
                await client.SendTextMessageAsync(
                       "chatID",
                       $"Человек меняет время от которого работает звонок." +
                       $"\nЕго зовут: {msg.Chat.FirstName}" +
                       $"\nВ телеграме известен как: {msg.From}");
                Console.WriteLine(DateTime.Now);
                Console.WriteLine($"Человек меняет время от которого работает звонок." +
                    $"\nЕго зовут: {msg.Chat.FirstName}" +
                    $"\nВ телеграме известен как: {msg.From}\n");
            }
            // в консоль | имени нет
            else
            {
                await client.SendTextMessageAsync(
                       "chatID",
                       $"Человек меняет время от которого работает звонок." +
                        $"\nЕго зовут: !Имя скрыто!");
                Console.WriteLine(DateTime.Now);
                Console.WriteLine($"Человек меняет время от которого работает звонок." +
                    $"\nЕго зовут: !Имя скрыто!\n");
            }
        }
        // метод действий на команду !Звонок!
        private static async void DoorRing(Telegram.Bot.Types.Message msg) {
            // если работаем сейчас
            if (DateTime.Now.Hour > HourOfStart && DateTime.Now.Hour < HourOfStop)
            {
                // в чат 
                await client.SendTextMessageAsync( 
                    msg.Chat.Id, "Вы нажали на кнопку звонка!" +
                    "\n\nВ доме зазвучала" +
                    "\nмелодия звонка," +
                    "\nесли мы её услышим," +
                    "\nмы вам откроем." +
                    "\n\nСпасибо за ожидание!" );
                // отбираем клавиатуру
                await client.SendTextMessageAsync(
                    msg.Chat.Id,
                    $"Позвонить в звонок снова вы сможете через {WaitTimeSec} сек",
                    replyMarkup: new ReplyKeyboardRemove()); ;
                // в беседу и в консоль | имя есть 
                if (msg.From != null && msg.Chat.FirstName != null)
                {
                    await client.SendTextMessageAsync(
                        "chatID",
                        $"У ворот стоит человек.\n\nПозвонил в звонок." +
                        $"\nЕго зовут: {msg.Chat.FirstName}" +
                        $"\nВ телеграме известен как: {msg.From}");
                    Console.WriteLine(DateTime.Now);
                    userTime = DateTime.Now;
                    Console.WriteLine($"У ворот стоит человек." +
                        $"\nПозвонил в звонок." +
                        $"\nЕго зовут: {msg.Chat.FirstName}" +
                        $"\nВ телеграме известен как: {msg.From}\n");
                }
                // в беседу и в консоль | имени нет
                else
                {
                    await client.SendTextMessageAsync(
                        "chatID",
                        $"У ворот стоит человек.\n\nПозвонил в звонок." +
                        $"\nЕго зовут: !Имя скрыто!");
                    Console.WriteLine(DateTime.Now);
                    Console.WriteLine($"У ворот стоит человек." +
                        $"\nПозвонил в звонок." +
                        $"\nЕго зовут: !Имя скрыто!\n");
                }
            }
            // если не работаем сейчас
            else
            {
                // в чат 
                await client.SendTextMessageAsync(
                    msg.Chat.Id, "Вы нажали на кнопку звонка!" +
                    "\n\nНо в доме не зазвучала" +
                    "\nмелодия звонка," +
                    "\nмы её не услышим," +
                    "\nмы вам не откроем." +
                    "\n\nСпасибо за ожидание!");
                // отбираем клавиатуру
                await client.SendTextMessageAsync(
                   msg.Chat.Id,
                   $"Позвонить в звонок снова вы сможете через {WaitTimeSec} сек",
                   replyMarkup: new ReplyKeyboardRemove()); ;
                // в беседу и в консоль | имя есть 
                if (msg.From != null && msg.Chat.FirstName != null)
                { // добавить сюда группы юзеров
                    await client.SendTextMessageAsync(
                        "chatID",
                        $"У ворот стоит человек.\n\nПозвонил в звонок." +
                        "\nНо позвонил в нерабочее время" +
                        $"\nЕго зовут: {msg.Chat.FirstName}" +
                        $"\nВ телеграме известен как: {msg.From}");
                    Console.WriteLine(DateTime.Now);
                    userTime = DateTime.Now;
                    Console.WriteLine($"У ворот стоит человек." +
                        $"\nПозвонил в звонок." +
                        "\nНо позвонил в нерабочее время" +
                        $"\nЕго зовут: {msg.Chat.FirstName}" +
                        $"\nВ телеграме известен как: {msg.From}\n");
                }
                // в беседу и в консоль | имени нет
                else
                {
                    await client.SendTextMessageAsync(
                        "chatID",
                        $"У ворот стоит человек.\n\nПозвонил в звонок." +
                        $"\nЕго зовут: !Имя скрыто!");
                    Console.WriteLine(DateTime.Now);
                    Console.WriteLine($"У ворот стоит человек." +
                        $"\nПозвонил в звонок." +
                        $"\nЕго зовут: !Имя скрыто!\n");
                }
            }
        }
        // метод воспроизведения музыки сервером под управлением Windows при нажатии звонка 
        private static void Beem() {
            /*
            System.IO.Stream str = Properties.Resources.mySoundFile;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
            */
        }
        // метод действий на отсутствие команд
        private static async void MessageForHouse(Telegram.Bot.Types.Message msg) {
            // в чат
            await client.SendTextMessageAsync(
                msg.Chat.Id, "Спасибо!\n\nВаше обращение принято!" +
                "\nМы ответим вам в ближайшее время.",
                replyToMessageId: msg.MessageId);
            // в беседу и в консоль | имя есть 
            if (msg.From != null && msg.Chat.FirstName != null) {
                await client.SendTextMessageAsync(
                    "chatID",
                    $"У ворот стоит человек.\n\nОн написал: {msg.Text}" +
                    $"\nЕго зовут: {msg.Chat.FirstName}" +
                    $"\nВ телеграме известен как: {msg.From}");
                Console.WriteLine(DateTime.Now);
                Console.WriteLine($"У ворот стоит человек." +
                    $"\nОн написал: {msg.Text}" +
                    $"\nЕго зовут: {msg.Chat.FirstName}" +
                    $"\nВ телеграме известен как: {msg.From}\n");
            }
            // в беседу и в консоль | имени нет
            else {
                await client.SendTextMessageAsync(
                    "chatID",
                    $"У ворот стоит человек.\n\nОн написал: {msg.Text}" +
                    $"\nЕго зовут: !Имя скрыто!");
                Console.WriteLine(DateTime.Now);
                Console.WriteLine($"У ворот стоит человек.\n\nОн написал: {msg.Text}" +
                    $"\nЕго зовут: !Имя скрыто!\n");
            }
        }
        // метод действия при неправильном вводе
        private static async void IncorrectEnter(Telegram.Bot.Types.Message msg) {
            // в чат
            await client.SendTextMessageAsync(msg.Chat.Id, "Вы отправили" +
                "\nпустое или недопустимое сообщение");
            // в консоль | имя есть
            if (msg.From != null && msg.Chat.FirstName != null) {
                Console.WriteLine(DateTime.Now);
                Console.WriteLine($"Отправили пустое или недопустимое сообщение" +
                $"\nОтправителя зовут: {msg.Chat.FirstName}" +
                $"\nВ телеграме известен как: {msg.From}\n");
            }
            // в консоль | имени нет
            else {
                Console.WriteLine(DateTime.Now);
                Console.WriteLine($"Отправили пустое или недопустимое сообщение" +
                $"\nОтправителя зовут: !Имя скрыто!\n");
            }
        }
        // метод общения бота и пользователя
        private static void OnMessageHandler(object sender, MessageEventArgs e) {
            var msg = e.Message; // переменная для сообщения
            // общение и звонок коррректный ввод
            if (msg.Text != null) {
                switch (msg.Text) {
                    // выключение бота 
                    case "password/stop":
                        Stop(msg); // метод действий на команду /stop
                        break;
                    // первое знакомство
                    case "/start":
                        Start(msg);// метод действий на команду /start
                        break;
                    // изменить время от
                    case "password/changeTimeFrom":
                        ChangeTimeFrom(msg); // метод действий на команду /changeFrom
                        break;
                    // изменить время до
                    case "password/changeTimeTo":
                        ChangeTimeTo(msg); // метод действий на команду /changeTo
                        break;
                    // звонок раньше 30 сек
                    case "!Нажмите через 30 секунд!":
                        break;
                    // звонок
                    case "!Звонок!":
                        DoorRing(msg); // метод действий на команду !Звонок!
                        break;
                    // сообщение для дома
                    default:
                        MessageForHouse(msg); // метод действий на отсутствие команд
                        break;
                } // switch
            }
            // некорректный ввод
            else{ IncorrectEnter(msg); }
        }
        // метод создания кнопки звонка
        private static IReplyMarkup GetButtonsCall() {
            //return new ReplyKeyboardMarkup(Text = "!Звонок!" , Row_width = 1, resize_keyboard = True, one_time_keyboard = True)
                return new ReplyKeyboardMarkup {
                    Keyboard = new List<List<KeyboardButton>> {
                    new List<KeyboardButton> { new KeyboardButton { Text = "!Звонок!" } }
                } // Keyboard 
                }; // ReplyKeyboardMarkup
        }
        // метод создания времени работы на клавиатуре
        private static IReplyMarkup GetHoursOfWorsButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>> {
                    new List<KeyboardButton> { new KeyboardButton {Text = $"!Позвонить в звонок вы сможете c {hourOfStart} до {hourOfStop}.!" } }
                } // Keyboard 
            }; // ReplyKeyboardMarkup
        }
        // метод отбирания кнопок у пользователя
        private static IReplyMarkup HideButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>> {
                    new List<KeyboardButton> { new KeyboardButton {Text = "!Нажмите через 30 секунд!"} }
                } // Keyboard 
            }; // ReplyKeyboardMarkup
        }

        // метод создания кнопок изменения часа старта звонка
        private static IReplyMarkup ChangeTimeFromButtons() {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>> {
                    new List<KeyboardButton> { new KeyboardButton {Text = "1"} }
                } // Keyboard 
            }; // ReplyKeyboardMarkup 
        }
        // метод создания кнопок изменения часа конца звонка
        private static IReplyMarkup ChangeTimeToButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>> {
                    new List<KeyboardButton> { new KeyboardButton {Text = "1"} }
                } // Keyboard 
            }; // ReplyKeyboardMarkup 
        }
    }
}

        // ---------------------------------------------------------------- //

        