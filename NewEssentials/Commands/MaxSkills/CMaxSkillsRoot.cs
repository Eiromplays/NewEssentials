﻿using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Permissions;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;

namespace NewEssentials.Commands.MaxSkills
{
    [Command("maxskills")]
    [CommandDescription("Grants maxskills")]
    [CommandSyntax("<none/player/all/kunii>")]
    [CommandActor(typeof(UnturnedUser))]
    public class CMaxSkillsRoot : UnturnedCommand
    {
        private readonly IPermissionChecker m_PermissionChecker;
        private readonly IStringLocalizer m_StringLocalizer;

        public CMaxSkillsRoot(IPermissionChecker permissionChecker,
            IStringLocalizer stringLocalizer, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_PermissionChecker = permissionChecker;
            m_StringLocalizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            string permission = "newess.maxskills";
            if (await m_PermissionChecker.CheckPermissionAsync(Context.Actor, permission) == PermissionGrantResult.Deny)
            {
                throw new NotEnoughPermissionException(Context, permission);
            }

            UnturnedUser uPlayer = (UnturnedUser) Context.Actor;
            await uPlayer.Player.skills.MaxAllSkills();

            await uPlayer.PrintMessageAsync(m_StringLocalizer["maxskills:granted"]);
        }
    }
}