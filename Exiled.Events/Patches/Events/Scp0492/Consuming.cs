// -----------------------------------------------------------------------
// <copyright file="Consuming.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Scp0492
{
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using API.Features.Core.Generic.Pools;
    using Exiled.Events.Attributes;
    using Exiled.Events.EventArgs.Scp0492;
    using HarmonyLib;
    using PlayerRoles.PlayableScps.Scp049;
    using PlayerRoles.PlayableScps.Scp049.Zombies;
    using PlayerRoles.Subroutines;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="RagdollAbilityBase{T}.ServerProcessCmd"/>
    /// to add <see cref="Handlers.Scp0492.ConsumingCorpse" /> event.
    /// </summary>
    [EventPatch(typeof(Handlers.Scp0492), nameof(Handlers.Scp0492.ConsumingCorpse))]
    [HarmonyPatch(typeof(ZombieConsumeAbility), nameof(ZombieConsumeAbility.ServerValidateBegin))]
    public class Consuming
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Pool.Get(instructions);

            int offset = 0;
            int index = newInstructions.FindLastIndex(i => i.opcode == OpCodes.Ldc_I4_0) + offset;

            newInstructions.InsertRange(index, new CodeInstruction[]
            {
                // this.Owner
                new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(newInstructions[index]),
                new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<ZombieRole>), nameof(StandardSubroutine<ZombieRole>.Owner))),

                // radgoll
                new(OpCodes.Ldarg_1),

                // true
                new(OpCodes.Ldc_I4_1),

                // ConsumingCorpseEventArgs ev = new(this.Owner, this.Ragdoll, true)
                new(OpCodes.Newobj, GetDeclaredConstructors(typeof(ConsumingCorpseEventArgs))[0]),
                new(OpCodes.Dup),

                // Handlers.Scp0492.OnConsumingCorpse(ev)
                new(OpCodes.Call, Method(typeof(Handlers.Scp0492), nameof(Handlers.Scp0492.OnConsumingCorpse))),

                // return ev.IsAllowed
                new(OpCodes.Callvirt, PropertyGetter(typeof(ConsumingCorpseEventArgs), nameof(ConsumingCorpseEventArgs.IsAllowed))),
                new(OpCodes.Ret),
            });

            foreach (CodeInstruction instruction in newInstructions)
                yield return instruction;

            ListPool<CodeInstruction>.Pool.Return(newInstructions);
        }
    }
}