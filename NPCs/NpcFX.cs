﻿using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WeaponOut.NPCs
{
    public class NpcFX : GlobalNPC
    {
        public override bool Autoload(ref string name)
        {
            return ModConf.enableWhips;
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (ModConf.enableWhips)
            {
                if (type == NPCID.Wizard)
                {
                    //add puzzling cutter if hardmode
                    if (Main.hardMode)
                    {
                        shop.item[nextSlot].SetDefaults(mod.ItemType<Items.Weapons.Whips.PuzzlingCutter>());
                        nextSlot++;
                    }
                }
            }
            if (ModConf.enableBasicContent)
            {
                if (type == NPCID.ArmsDealer)
                {
                    //add scrap salvo after mech
                    if (Main.hardMode && NPC.downedPlantBoss)
                    {
                        shop.item[nextSlot].SetDefaults(mod.ItemType<Items.Weapons.Basic.ScrapSalvo>());
                        nextSlot++;
                    }
                }
            }
            if (ModConf.enableFists)
            {
                if (type == NPCID.Clothier)
                {
                    //add headbands after bossess
                    if (NPC.downedBoss3)
                    {
                        shop.item[nextSlot].SetDefaults(mod.ItemType<Items.Armour.FistVeteranHead>());
                        nextSlot++;
                    }
                    if (NPC.downedGolemBoss)
                    {
                        shop.item[nextSlot].SetDefaults(mod.ItemType<Items.Armour.FistMasterHead>());
                        nextSlot++;
                    }
                }
            }
        }

        public override void SetupTravelShop(int[] shop, ref int nextSlot)
        {
            if (ModConf.enableFists)
            {
                int chance = 5;
                if (NPC.downedSlimeKing) chance--;
                if (NPC.downedBoss1) chance--;
                int taekwonBody = mod.ItemType<Items.Armour.FistPowerBody>();
                int boxingBody = mod.ItemType<Items.Armour.FistSpeedBody>();
                foreach (Player p in Main.player)
                {
                    if (p.armor[1].type == taekwonBody || p.armor[1].type == boxingBody)
                    {
                        chance -= 2;
                    }
                }
                if (Main.rand.Next(Math.Max(1,chance)) == 0)
                {
                    shop[nextSlot] = mod.ItemType<Items.Armour.FistDefBody>(); nextSlot++;
                    shop[nextSlot] = mod.ItemType<Items.Armour.FistDefLegs>(); nextSlot++;
                }
            }
        }

        // Plz... why's ID is being read 1 down???
        private int GetItemTypeHack(string itemType)
        {
            if (Main.netMode == 2)
            { return mod.ItemType(itemType) + 1; }
            return mod.ItemType(itemType);
        }
        public override void NPCLoot(NPC npc)
        {
            if (ModConf.enableFists)
            {
                if (npc.type == NPCID.GraniteGolem && Main.rand.Next(10) == 0)
                {
                    Item.NewItem(npc.position, npc.Size, GetItemTypeHack("FistsGranite"), 1, false, -1, false, false);
                    return;
                }
                if (npc.type == NPCID.BoneLee && Main.rand.Next(6) == 0)
                {
                    Item.NewItem(npc.position, npc.Size, GetItemTypeHack("GlovesLee"), 1, false, -1, false, false);
                    return;
                }

                // Bosses drop per-player
                // REGEX: mod.ItemType<Items.Accessories.([\w]*)>\(\)
                //      : GetItemTypeHack("$1")
                if (npc.boss)
                {
                    int itemType = -1;
                    if (Main.expertMode)
                    {
                        if (npc.type == NPCID.KingSlime)
                        { itemType = mod.ItemType("RoyalHealGel"); }

                        if (npc.type == NPCID.EyeofCthulhu)
                        { itemType = mod.ItemType("RushCharm"); }

                        if (npc.type >= NPCID.EaterofWorldsHead && npc.type <= NPCID.EaterofWorldsTail)
                        { itemType = mod.ItemType("DriedEye"); }

                        if (npc.type == NPCID.BrainofCthulhu)
                        { itemType = mod.ItemType("StainedTooth"); }

                        if (npc.type == NPCID.QueenBee)
                        { itemType = mod.ItemType("QueenComb"); }

                        if (npc.type == NPCID.SkeletronHead)
                        { itemType = mod.ItemType("FistsBone"); }

                        if (npc.type == NPCID.WallofFlesh)
                        { itemType = mod.ItemType("DemonBlood"); }

                        if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
                        {
                            int partner = NPCID.Spazmatism;
                            if (npc.type == NPCID.Spazmatism) partner = NPCID.Retinazer;
                            // Last eye standing
                            if (!NPC.AnyNPCs(partner))
                            {
                                itemType = mod.ItemType("ScrapActuator");
                            }
                        }

                        if (npc.type == NPCID.TheDestroyer)
                        { itemType = mod.ItemType("ScrapFrame"); }

                        if (npc.type == NPCID.SkeletronPrime)
                        { itemType = mod.ItemType("ScrapTransformer"); }

                        if (npc.type == NPCID.Plantera)
                        { itemType = mod.ItemType("LifeRoot"); }

                        if (npc.type == NPCID.Golem)
                        { itemType = mod.ItemType("GolemHeart"); }

                    }
                    // Modified from DropItemInstanced, only drop for people using fists
                    if (itemType > 0)
                    {
                        if (Main.netMode == 2) itemType++;
                        DropInstancedFistItem(npc, itemType);
                    }
                    itemType = -1;

                    bool chance = Main.rand.Next(5) == 0;
                    if (!chance)
                    {
                        foreach (Player p in Main.player)
                        {
                            if (!p.active) continue;
                            if (p.HeldItem.useStyle == ModPlayerFists.useStyle)
                            {
                                chance = Main.rand.Next(2) == 0 || Main.expertMode;
                                break;
                            }
                        }
                    }
                    if (chance)
                    {
                        if (npc.type == NPCID.KingSlime)
                        { itemType = mod.ItemType("FistsSlime"); }

                        if (npc.type == NPCID.Plantera)
                        { itemType = mod.ItemType("KnucklesPlantera"); }

                        if (npc.type == NPCID.MartianSaucerCore)
                        { itemType = mod.ItemType("FistsMartian"); }

                        if (npc.type == NPCID.DukeFishron)
                        { itemType = mod.ItemType("KnucklesDuke"); }

                        if (npc.type == NPCID.DD2Betsy)
                        { itemType = mod.ItemType("FistsBetsy"); }
                        /*
                        if (npc.type == NPCID.IceQueen) //TODO: set this up, also npc.boss
                        { itemType = mod.ItemType<Items.Weapons.Fists.FistsFrozen>(); }*/
                    }

                    // Modified from DropItemInstanced, only drop for people using fists
                    if (itemType > 0)
                    {
                        if (Main.netMode == 2) itemType++; 
                        DropInstancedFistItem(npc, itemType);
                    }
                }
            }
        }

        private static void DropInstancedFistItem(NPC npc, int itemType)
        {
            if (Main.netMode == 2)
            {
                int itemWho = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,
                    itemType, 1, true, -1, false, false);
                Main.itemLockoutTime[itemWho] = 54000; // This slot cannot be used again for a while
                foreach (Player player in Main.player)
                {
                    if (!player.active) continue;
                    if (npc.playerInteraction[player.whoAmI])
                    {// Player must've hit the NPC at least once
                        if (player.HeldItem.useStyle == ModPlayerFists.useStyle)
                        {// Is equipped with fists, probably punching at time of death
                            NetMessage.SendData(MessageID.InstancedItem, player.whoAmI, -1, null, itemWho, 0f, 0f, 0f, 0, 0, 0);
                            
                        }
                    }
                }
                Main.item[itemWho].active = false;
            }
            else if (Main.netMode == 0 && Main.LocalPlayer.HeldItem.useStyle == ModPlayerFists.useStyle)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,
                    itemType, 1, false, -1, false, false);
            }
        }

        #region The last straw against player-snapping bosses :(
        public override bool PreAI(NPC npc)
        {
            if (!npc.active || npc.life <= 0 || npc.lifeMax < 2000 || !npc.chaseable || npc.npcSlots <= 0f ||
                !npc.HasPlayerTarget) return true; // we don't deal in small fry and clones

            if (npc.boss || npc.GetBossHeadTextureIndex() >= 0)
            {
                PlayerFX pfx;
                foreach (Player p in Main.player)
                {
                    if (!p.active || p.dead) continue;
                    pfx = p.GetModPlayer<PlayerFX>();
                    if (pfx.ghostPosition && !pfx.FakePositionReal.Equals(default(Vector2)))
                    { p.position = pfx.FakePositionTemp; }
                }
            }
            return base.PreAI(npc);
        }

        public override void PostAI(NPC npc)
        {
            if (npc.lifeMax < 2000 || npc.npcSlots <= 0f) return; // like eater of worlds
            if (npc.boss || npc.GetBossHeadTextureIndex() >= 0)
            {
                PlayerFX pfx;
                foreach (Player p in Main.player)
                {
                    if (!p.active || p.dead) continue;
                    pfx = p.GetModPlayer<PlayerFX>();
                    if (pfx.ghostPosition && !pfx.FakePositionReal.Equals(default(Vector2)))
                    { p.position = pfx.FakePositionReal; }
                }
            }
        }

        #endregion
    }
}
/*
 
				if (Main.netMode == 2)
				{
					int num = Item.NewItem((int)Position.X, (int)Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y, itemType, itemStack, true, 0, false, false);
					Main.itemLockoutTime[num] = 54000;
					for (int i = 0; i < 255; i++)
					{
						if ((this.playerInteraction[i] || !interactionRequired) && Main.player[i].active)
						{
							NetMessage.SendData(90, i, -1, null, num, 0f, 0f, 0f, 0, 0, 0);
						}
					}
					Main.item[num].active = false;
				}
				else if (Main.netMode == 0)
				{
					Item.NewItem((int)Position.X, (int)Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y, itemType, itemStack, false, 0, false, false);
				}

     */
