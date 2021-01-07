using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Models.Commands;

namespace TgBotBull_Cow.Models.Commands
{
    public class Play : Command
    {
        ApplicationContext db = new ApplicationContext();
        public override string Name => "play";
        public int[] arrSave = new int[4];
        public int[] arrayAnswer = new int[4];
        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            Codes codes = new Codes();

            fillArray();

            string str = "";
            foreach (var item in arrayAnswer)
            {
                str += item;
            }

            codes.ChatId = (int)chatId;
            codes.Code = str;

            db.Codes.Add(codes);
            db.SaveChanges();

            string tutorial = "Игра началась!\n Быки и коровы — логическая игра, в ходе которой за несколько попыток игрок должен определить, что за число задумано." +
                              "Число 4-значное с неповторяющимися цифрами. Игрок отправляет 4-значное с неповторяющимися цифрами. Компьютер сообщает в ответ, сколько цифр угадано без совпадения с" +
                              "их позициями в тайном числе (то есть количество коров) и сколько угадано вплоть до позиции в тайном числе (то есть количество быков). Например: " +
                              "Задумано тайное число «3219». Попытка: «2310». Результат: две «коровы» (две цифры: «2» и «3» — угаданы на неверных позициях) и один «бык» (одна цифра «1» угадана вплоть до позиции)." +
                              "\nВведите 4-значное число с неповторяющимися цифрами! Удачи!";

            await client.SendTextMessageAsync(chatId, tutorial);
        }

        public int randomNumber()
        {
            Random code = new Random();
            return code.Next(0, 10);
        }
        
        public void fillArray()
        {
            for (int i = 0; i < 4; i++)
            {
                arrSave[i] = randomNumber();

                for (int j = 0; j < 4; j++)
                {
                    if (arrayAnswer[j] == arrSave[i])
                    {
                        arrSave[i] = randomNumber();
                        j = -1;
                    }
                }
                arrayAnswer[i] = arrSave[i];
            }
        }
    }
}