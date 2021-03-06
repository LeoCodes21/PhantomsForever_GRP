﻿using Discord.Commands;
using Discord.WebSocket;
using PhantomsForever_GRP.Core.Database;
using PhantomsForever_GRP.Core.Discord.ConditionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D = Discord;

namespace PhantomsForever_GRP.Core.Discord.Commands
{
    [Group("mod")]
    [RequireModerator]
    public class ModeratorCommands : ModuleBase<SocketCommandContext>
    {
        [Command("mute")]
        [Summary("Mutes an idiot")]
        public async Task MuteAsync([Summary("The user to mute")] SocketUser user)
        {
            var u = Context.Guild.GetUser(user.Id);
            var roles = u.Roles;
            string r = "";
            foreach (var role in roles)
                r += role.Name + ";";
            DiscordDatabaseHandler.MuteUser(u.Id.ToString(), r, "undefined");
            await u.RemoveRolesAsync(roles.Where(x => x.IsEveryone != true));
            await u.AddRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == "Muted"));
            PhantomsForeverBot.Instance.Log(Context.User.Username + " muted " + user.Username + " forever");
            await ReplyAsync("Muted " + user.Username);
        }
        [Command("mute")]
        [Summary("Mutes an idiot for a specified time")]
        public async Task MuteAsync([Summary("The user to mute")] SocketUser user, string time, string btime)
        {
            var dt = DateTime.Now;
            var t = int.Parse(time);
            switch (btime.ToLower())
            {
                case "second":
                case "seconds":
                    dt = dt.AddSeconds(t);
                    break;
                case "minute":
                case "minutes":
                    dt = dt.AddMinutes(t);
                    break;
                case "hour":
                case "hours":
                    dt = dt.AddHours(t);
                    break;
                case "day":
                case "days":
                    dt = dt.AddDays(t);
                    break;
                case "week":
                case "weeks":
                    dt = dt.AddDays(t * 7);
                    break;
                case "month":
                case "months":
                    dt = dt.AddMonths(t);
                    break;
                case "year":
                case "years":
                    dt = dt.AddYears(t);
                    break;
                default:
                    await ReplyAsync("The time you specified is invalid, " + Context.User.Mention);
                    return;
            }
            var u = Context.Guild.GetUser(user.Id);
            var roles = u.Roles;
            string r = "";
            foreach (var role in roles)
                r += role.Name + ";";
            DiscordDatabaseHandler.MuteUser(u.Id.ToString(), r, dt.ToString());
            await u.RemoveRolesAsync(roles.Where(x => x.IsEveryone != true));
            await u.AddRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == "Muted"));
            PhantomsForeverBot.Instance.Log(Context.User.Username + " muted " + user.Username + " untill " + dt.ToString());
            await ReplyAsync("Muted " + user.Username + " untill " + dt.ToString());
        }
        [Command("unmute")]
        [Summary("Unmutes an idiot, needs a reason")]
        public async Task UnmuteAsync([Summary("The user to unmute")] SocketUser user, [Summary("The reason to unmute")][RemainderAttribute] string reason)
        {
            DiscordDatabaseHandler.Unmute(user.Id.ToString());
            PhantomsForeverBot.Instance.Log(Context.User.Username + " unmuted " + user.Username + " and gave reason: " + reason);
            await ReplyAsync("Unmuted " + user.Username);
        }
    }
}