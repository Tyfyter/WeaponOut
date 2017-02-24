﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using WeaponOut.Items.Weapons.UseStyles;

namespace WeaponOut.Items.Weapons.Fists
{
    public class KnucklesDungeon : ModItem
    {
        public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
        {
            return ModConf.enableFists;
        }

        private FistStyle fist;
        public FistStyle Fist
        {
            get
            {
                if (fist == null)
                {
                    fist = new FistStyle(item, 2);
                }
                return fist;
            }
        }
        public override void SetDefaults()
        {
            item.name = "Cerulean Claws";
            item.toolTip = "<right> at full combo power to unleash spirit";
            item.useStyle = FistStyle.useStyle;
            item.autoReuse = true;
            item.useAnimation = 22;
            item.useTime = 22;

            item.width = 20;
            item.height = 20;
            item.damage = 25;
            item.knockBack = 3.5f;
            item.UseSound = SoundID.Item7;

            item.shoot = mod.ProjectileType<Projectiles.SpiritBlast>();
            item.shootSpeed = 14f;

            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = 3;
            item.noUseGraphic = true;
            item.melee = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Fist.ModifyTooltips(tooltips, mod);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldenKey, 1);
            recipe.AddIngredient(ItemID.Bone, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        
        public override void HoldItem(Player player)
        {
            if (Fist.ExpendCombo(player, true) > 0)
            {
                HeliosphereEmblem.DustVisuals(player, 20, 0.9f);
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return Fist.ExpendCombo(player) > 0;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            damage *= 3;
            knockBack *= 1.5f;
            return player.altFunctionUse > 0;
        }

        public override bool UseItemFrame(Player player)
        {
            Fist.UseItemFrame(player);
            return true;
        }
        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            noHitbox = Fist.UseItemHitbox(player, ref hitbox, 22, 9f, 8f, 8f);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            int combo = Fist.OnHitNPC(player, target, true);
            if (combo != -1)
            {
                if (combo % Fist.punchComboMax == 0)
                {
                    // Ready flash
                    PlayerFX.ItemFlashFX(player);
                }
            }
        }
    }
}
