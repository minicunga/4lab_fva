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
    public class Game : Command
    {
        ApplicationContext db = new ApplicationContext();
        public Game()
        {
            db.Codes.Load();
        }
        public override string Name => "game";

        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            int res;
            bool isInt = Int32.TryParse(message.Text, out res);
            if (!isInt)
            {
                await client.SendTextMessageAsync(chatId, "Введите 4-значное число с неповторяющимися цифрами!");
                return;
            }
            string code = "";
            string result = "";
            char[] codeMess = message.Text.ToCharArray();

            foreach (var item in db.Codes)
            {
                if (item.ChatId == chatId)
                {
                    code = item.Code;
                    break;
                }
            }
            
            if (string.IsNullOrEmpty(code))
            {
                await client.SendTextMessageAsync(chatId, "Для начала вызовите команду /play@BowAndCow_bot");
            }
            else
            {
                char[] codeAnswer = code.ToCharArray();

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (codeMess[i] == codeMess[j] && i != j)
                        {
                            await client.SendTextMessageAsync(chatId, "Цифры повторяются! Введите разные цифры");
                            return;
                        }
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (codeAnswer[i] == codeMess[i])
                    {
                        result += "БЫК-";
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (codeAnswer[j] == codeMess[i] && j != i)
                        {
                            result += "КОРОВА-";
                        }
                    }
                }
                await client.SendTextMessageAsync(chatId, result);

                if (result == "БЫК-БЫК-БЫК-БЫК-")
                {
                    await client.SendTextMessageAsync(chatId, "Поздравляю! Ты угадал весь код! Код был: " + code + "\n Чтобы начать новую игру напиши /play");

                    foreach (var item in db.Codes)
                    {
                        if (item.ChatId == chatId)
                        {
                            db.Codes.Remove(item);
                            db.SaveChanges();
                            break;
                        }
                    }                   
                }

                if (result == "")
                {
                    await client.SendTextMessageAsync(chatId, "Нет сопадений");
                }


            }
        }
    }
}