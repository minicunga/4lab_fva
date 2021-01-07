using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Telegram.Bot.Types;
using TelegramBot.Models;

namespace TelegramBot.Controllers
{
    public class MessageController : ApiController
    {
        [Route(@"api/message/update")]
        public async Task<OkResult> Update([FromBody] Update update)
        {
            var commands = Bot.Commands;
            var message = update.Message;
            var client = await Bot.Get();
            bool game = true;

            foreach (var command in commands)
            {
                if (command.Contains(message.Text))
                {
                    await Task.Run(() => command.Execute(message, client));
                    game = false;
                    break;
                }
            }

            if (game)
            {
                await Task.Run(() => commands.ElementAt(commands.Count - 1).Execute(message, client));
            }

            return Ok();
        }
    }
}
